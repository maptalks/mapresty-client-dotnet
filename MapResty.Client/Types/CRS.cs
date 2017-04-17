using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MapResty.Client.Types
{
    /// <summary>
    /// 坐标参考系
    /// </summary>
    public class CRS : IEquatable<CRS>
    {
        /// <summary>
        /// 类型。如proj4，使用proj4的字符串表示
        /// </summary>
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type { get; set; }

        /// <summary>
        /// 类型相关的属性。如{ proj: "+proj=longlat +datum=WGS84 +no_defs" }
        /// </summary>
        [JsonProperty(PropertyName = "properties", Required = Required.Always)]
        public Dictionary<string, object> Properties { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as CRS);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(CRS other)
        {
            return Equals(this, other);
        }

        public bool Equals(CRS left, CRS right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }

            if (left.Type != right.Type)
            {
                return false;
            }

            var leftIsNull = ReferenceEquals(null, left.Properties);
            var rightIsNull = ReferenceEquals(null, right.Properties);
            var bothAreMissing = leftIsNull && rightIsNull;

            if (bothAreMissing || leftIsNull != rightIsNull)
            {
                return bothAreMissing;
            }

            foreach (var item in left.Properties)
            {
                if (!right.Properties.ContainsKey(item.Key))
                {
                    return false;
                }
                if (!object.Equals(item.Value, right.Properties[item.Key]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(CRS left, CRS right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }
            return left != null && left.Equals(right);
        }

        public static bool operator !=(CRS left, CRS right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 创建proj4类型的CRS对象
        /// </summary>
        /// <param name="proj4str">proj4的字符串</param>
        /// <returns>proj4类型的CRS对象</returns>
        public static CRS CreateProj4(string proj4str)
        {
            return new CRS
            {
                Type = "proj4",
                Properties = new Dictionary<string, object>
                {
                    { "proj", proj4str }
                }
            };
        }

        /// <summary>
        /// proj4: EPSG4326
        /// </summary>
        public static CRS EPSG4326 = CreateProj4("+proj=longlat +datum=WGS84 +no_defs");

        /// <summary>
        /// proj4: EPSG3857
        /// </summary>
        public static CRS EPSG3857 = CreateProj4("+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext +no_defs");

        /// <summary>
        /// proj4: BD09LL，百度经纬度
        /// </summary>
        public static CRS BD09LL = CreateProj4("+proj=longlat +datum=BD09");

        /// <summary>
        /// proj4: GCJ02，国测局02
        /// </summary>
        public static CRS GCJ02 = CreateProj4("+proj=longlat +datum=GCJ02");

        /// <summary>
        /// proj4: WGS84
        /// </summary>
        public static CRS WGS84 = EPSG4326;
    }
}
