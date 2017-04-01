using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    public class LayerField
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
    }
}
