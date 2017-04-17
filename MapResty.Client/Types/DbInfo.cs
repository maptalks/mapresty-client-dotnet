using Newtonsoft.Json;
using System;

namespace MapResty.Client.Types
{
    /// <summary>
    /// 空间数据库信息
    /// </summary>
    public class DbInfo : IEquatable<DbInfo>
    {
        /// <summary>
        /// 数据库名字
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// 所使用的坐标参考系
        /// </summary>
        [JsonProperty(PropertyName = "crs", Required = Required.Always)]
        public CRS CRS { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty(PropertyName = "version", Required = Required.Always)]
        public string Version { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as DbInfo);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(DbInfo other)
        {
            return Equals(this, other);
        }

        public bool Equals(DbInfo left, DbInfo right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }

            if (left.Name != right.Name)
            {
                return false;
            }
            if (left.Version != right.Version)
            {
                return false;
            }
            if (left.CRS != right.CRS)
            {
                return false;
            }

            return true;
        }

        public static bool operator ==(DbInfo left, DbInfo right)
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

        public static bool operator !=(DbInfo left, DbInfo right)
        {
            return !(left == right);
        }
    }
}
