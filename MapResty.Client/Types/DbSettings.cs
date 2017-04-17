using Newtonsoft.Json;
using System;

namespace MapResty.Client.Types
{
    /// <summary>
    /// 初始化参数
    /// </summary>
    public class DbSettings : IEquatable<DbSettings>
    {
        /// <summary>
        /// 空间参考系
        /// </summary>
        [JsonProperty(PropertyName = "crs")]
        public CRS CRS { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as DbSettings);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(DbSettings other)
        {
            return Equals(this, other);
        }

        public bool Equals(DbSettings left, DbSettings right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }

            if (left.CRS != right.CRS)
            {
                return false;
            }

            return true;
        }

        public static bool operator ==(DbSettings left, DbSettings right)
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

        public static bool operator !=(DbSettings left, DbSettings right)
        {
            return !(left == right);
        }
    }
}
