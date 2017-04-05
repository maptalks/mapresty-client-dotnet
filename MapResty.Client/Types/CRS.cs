using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MapResty.Client.Types
{
    public class CRS : IEquatable<CRS>
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type { get; set; }

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

        public static CRS EPSG4326 = CreateProj4("+proj=longlat +datum=WGS84 +no_defs");
        public static CRS EPSG3857 = CreateProj4("+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext +no_defs");
        public static CRS BD09LL = CreateProj4("+proj=longlat +datum=BD09");
        public static CRS GCJ02 = CreateProj4("+proj=longlat +datum=GCJ02");
        public static CRS WGS84 = EPSG4326;
    }
}
