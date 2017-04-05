using Newtonsoft.Json;
using System;

namespace MapResty.Client.Types
{
    public class DbInfo : IEquatable<DbInfo>
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "crs", Required = Required.Always)]
        public CRS CRS { get; set; }

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
