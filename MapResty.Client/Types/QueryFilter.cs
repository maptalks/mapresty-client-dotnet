using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    /// <summary>
    /// 查询过滤器
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// 全部自定义属性,即查询结果返回所有的自定义属性。
        /// 如: queryFilter.setResultFields(QueryFilter.ALL_FIELDS);
        /// </summary>
        public static string[] ALL_FIELDS = new string[] { "*" };

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryFilter()
        {
            ReturnGeometry = true;
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }


        /// <summary>
        /// 空间过滤filter
        /// </summary>
        [JsonProperty(PropertyName = "spatialFilter")]
        public SpatialFilter SpatialFilter { get; set; }

        /// <summary>
        /// 结果坐标参考系
        /// </summary>
        [JsonProperty(PropertyName = "crs")]
        public CRS CRS { get; set; }

        /// <summary>
        /// 要返回的图层表字段数组
        /// </summary>
        [JsonProperty(PropertyName = "resultFields")]
        public string[] ResultFields { get; set; }

        /// <summary>
        /// 是否返回Geometry。
        /// 默认返回。
        /// </summary>
        [JsonProperty(PropertyName = "returnGeometry")]
        public bool ReturnGeometry { get; set; }
    }
}
