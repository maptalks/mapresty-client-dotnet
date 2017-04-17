using Newtonsoft.Json;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;

namespace MapResty.Client.Types
{
    public class SpatialFilter
    {
        /// <summary>
        /// 相交关系
        /// </summary>
        public static int RELATION_INTERSECT = 0;

        /// <summary>
        /// 包含关系
        /// </summary>
        public static int RELATION_CONTAIN = 1;

        /// <summary>
        /// 分离关系
        /// </summary>
        public static int RELATION_DISJOINT = 2;

        /// <summary>
        /// 重叠关系
        /// </summary>
        public static int RELATION_OVERLAP = 3;

        /// <summary>
        /// 相切关系
        /// </summary>
        public static int RELATION_TOUCH = 4;

        /// <summary>
        /// 被包含关系
        /// </summary>
        public static int RELATION_WITHIN = 5;

        /// <summary>
        /// (非标准)相交但不包含关系即within 或 overlap
        /// </summary>
        public static int RELATION_INTERECTNOTCONTAIN = 100;

        /// <summary>
        /// (非标准)包含中心点
        /// </summary>
        public static int RELATION_CONTAINCENTER = 101;

        /// <summary>
        /// (非标准)中心点被包含
        /// </summary>
        public static int RELATION_CENTERWITHIN = 102;

        /// <summary>
        /// 空间关系比较的geometry对象
        /// </summary>
        [JsonProperty(PropertyName = "geometry")]
        [JsonConverter(typeof(GeometryConverter))]
        public IGeometryObject Geometry { get; set; }

        /// <summary>
        /// 空间关系，取值范围从上面定义的关系常量中取得
        /// </summary>
        [JsonProperty(PropertyName = "relation")]
        public int Relation { get; set; }

        /// <summary>
        /// Geometry的坐标参考系
        /// </summary>
        [JsonProperty(PropertyName = "crs")]
        public CRS CRS { get; set; }
    }
}
