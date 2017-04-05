using Newtonsoft.Json;
using System;

namespace MapResty.Client.Types
{
    public class LayerField : IEquatable<LayerField>
    {
        /// <summary>
        /// 属性名
        /// </summary>
        [JsonProperty(PropertyName = "fieldName", Required = Required.Always)]
        public string FieldName { get; set; }

        /// <summary>
        /// 数据类型，例如VARCHAR, INT
        /// </summary>
        [JsonProperty(PropertyName = "dataType", Required = Required.Always)]
        public string DataType { get; set; }

        /// <summary>
        /// 数据位宽，例如VARCHAR(32)中的32
        /// </summary>
        [JsonProperty(PropertyName = "fieldSize")]
        public int FieldSize { get; set; }

        /// <summary>
        /// 小数点后位宽，例如NUMBER(10,3)中的3
        /// </summary>
        [JsonProperty(PropertyName = "decimalSize")]
        public int DecimalSize { get; set; }

        /// <summary>
        /// 是否可以为空，如果为0，则可以为空，如果为1，则不能为空
        /// </summary>
        [JsonProperty(PropertyName = "notNull")]
        public int NotNull { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as LayerField);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(LayerField other)
        {
            return Equals(this, other);
        }

        public bool Equals(LayerField left, LayerField right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }

            if (left.FieldName != right.FieldName)
            {
                return false;
            }
            if (left.DataType != left.DataType)
            {
                return false;
            }
            if (left.FieldSize != right.FieldSize)
            {
                return false;
            }
            if (left.DecimalSize != right.DecimalSize)
            {
                return false;
            }
            if (left.NotNull != right.NotNull)
            {
                return false;
            }

            return true;
        }

        public static bool operator ==(LayerField left, LayerField right)
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

        public static bool operator !=(LayerField left, LayerField right)
        {
            return !(left == right);
        }
    }
}
