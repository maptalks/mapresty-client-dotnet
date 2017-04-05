using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    public class QueryFilter
    {
        /**
         * 全部自定义属性,即查询结果返回所有的自定义属性 例子:  queryFilter.setResultFields(QueryFilter.ALL_FIELDS);
        */
        public static string[] ALL_FIELDS = new string[] { "*" };

        public QueryFilter()
        {
            ReturnGeometry = true;
        }

        /**
         * 查询条件
         */
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        /**
         * 空间过滤filter
         */
        [JsonProperty(PropertyName = "spatialFilter")]
        public SpatialFilter SpatialFilter { get; set; }

        /**
         * 结果坐标系
         */
        [JsonProperty(PropertyName = "crs")]
        public CRS CRS { get; set; }

        /**
         * 要返回的自定义属性
         */
        [JsonProperty(PropertyName = "resultFields")]
        public string[] ResultFields { get; set; }

        /**
         * 是否返回Geometry
         */
        [JsonProperty(PropertyName = "returnGeometry")]
        public bool ReturnGeometry { get; set; }
    }
}
