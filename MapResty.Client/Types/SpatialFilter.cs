using Newtonsoft.Json;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;

namespace MapResty.Client.Types
{
    public class SpatialFilter
    {
        // 图形关系常量定义
        // 相交关系
        public static int RELATION_INTERSECT = 0;
        // 包含关系
        public static int RELATION_CONTAIN = 1;
        // 分离关系
        public static int RELATION_DISJOINT = 2;
        // 重叠关系
        public static int RELATION_OVERLAP = 3;
        // 相切关系
        public static int RELATION_TOUCH = 4;
        // 被包含关系
        public static int RELATION_WITHIN = 5;
        // 以下是非标准的图形关系
        /**
         * 相交但不包含关系即within 或 overlap
         */
        public static int RELATION_INTERECTNOTCONTAIN = 100;
        /**
         * 包含中心点
         */
        public static int RELATION_CONTAINCENTER = 101;
        /**
         * 中心点被包含
         */
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
        
        [JsonProperty(PropertyName = "crs")]
        public CRS CRS { get; set; }
    }
}
