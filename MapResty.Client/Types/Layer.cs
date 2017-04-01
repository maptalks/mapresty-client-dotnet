using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    public class Layer
    {
        public static string TYPE_DB_TABLE = "db_table";
        public static string TYPE_DB_VIEW = "db_view";
        public static string TYPE_DB_SPATIAL_TABLE = "db_spatial_table";
        public static string TYPE_DB_SPATIAL_VIEW = "db_spatial_view";
        public static string TYPE_FILE_SHP = "file_shp";

        //---------以下定义了配置项常量,即properties属性中的配置项名称
        /**
         * mysql的数据表引擎, innodb或myISAM
         */
        public static string PROPERTY_MYSQL_ENGINE = "engine";
        public static string PROPERTY_MYSQL_ENGINE_DEFAULT = "MyISAM";

        /**
         * shapefile的encoding
         */
        public static string PROPERTY_SHP_ENCODING = "encoding";
        public static string PROPERTY_SHP_ENCODING_DEFAULT = "utf-8";

        /**
         * 载入的DBF属性列表,属性以逗号分隔,默认为空,即不载入
         */
        public static string PROPERTY_SHP_PROPERTY = "property";
        public static string PROPERTY_SHP_PROPERTY_DEFAULT = null;

        /**
         * ShapeFile的坐标类型
         */
        public static string PROPERTY_SHP_CRS = "crs";

        /**
         * 是否建立索引
         */
        public static string PROPERTY_CREATE_INDEX = "createindex";
        public static bool PROPERTY_CREATE_INDEX_DEFAULT = true;
        // ------------------------------------------------------

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, object> Properties { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<LayerField> Fields { get; set; }
    }
}
