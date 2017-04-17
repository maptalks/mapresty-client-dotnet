using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    /// <summary>
    /// 图层信息
    /// </summary>
    public class Layer : IEquatable<Layer>
    {
        /// <summary>
        /// 图层类型：表(table)
        /// </summary>
        public static string TYPE_DB_TABLE = "db_table";

        /// <summary>
        /// 图层类型：视图(view)
        /// </summary>
        public static string TYPE_DB_VIEW = "db_view";

        /// <summary>
        /// 图层类型：空间表(spatial table)
        /// </summary>
        public static string TYPE_DB_SPATIAL_TABLE = "db_spatial_table";

        /// <summary>
        /// 图层类型：空间表视图(view based on spatial table)
        /// </summary>
        public static string TYPE_DB_SPATIAL_VIEW = "db_spatial_view";

        /// <summary>
        /// 图层类型：shp文件(shpaefile)
        /// </summary>
        public static string TYPE_FILE_SHP = "file_shp";

        /// <summary>
        /// 属性名。mysql的数据表引擎，innodb或myISAM
        /// </summary>
        public static string PROPERTY_MYSQL_ENGINE = "engine";

        /// <summary>
        /// 属性名。shapefile的encoding
        /// </summary>
        public static string PROPERTY_SHP_ENCODING = "encoding";

        /// <summary>
        /// 属性名。载入的DBF属性列表，属性以逗号分隔。
        /// 默认为空，即不载入。
        /// </summary>
        public static string PROPERTY_SHP_PROPERTY = "property";

        /// <summary>
        /// 属性名。ShapeFile的坐标参考系
        /// </summary>
        public static string PROPERTY_SHP_CRS = "crs";

        /// <summary>
        /// 属性名。是否建立索引
        /// </summary>
        public static string PROPERTY_CREATE_INDEX = "createindex";

        /// <summary>
        /// 图层ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// 图层名字
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 图层类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 图层数据源(表名，视图名，文件名)
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        /// <summary>
        /// 图层特定属性
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// 图层表字段列表
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public List<LayerField> Fields { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as Layer);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Layer other)
        {
            return Equals(this, other);
        }

        public bool Equals(Layer left, Layer right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right))
            {
                return false;
            }

            if (left.Id != right.Id)
            {
                return false;
            }
            if (left.Name != right.Name)
            {
                return false;
            }
            if (left.Type != right.Type)
            {
                return false;
            }
            if (left.Source != right.Source)
            {
                return false;
            }

            var leftIsNull = ReferenceEquals(null, left.Properties);
            var rightIsNull = ReferenceEquals(null, right.Properties);
            var bothAreMissing = leftIsNull && rightIsNull;

            if (bothAreMissing || leftIsNull != rightIsNull)
            {
                return bothAreMissing;
            }

            foreach (var item in left.Properties)
            {
                if (!right.Properties.ContainsKey(item.Key))
                {
                    return false;
                }
                if (!object.Equals(item.Value, right.Properties[item.Key]))
                {
                    return false;
                }
            }

            leftIsNull = ReferenceEquals(null, left.Fields);
            rightIsNull = ReferenceEquals(null, right.Fields);
            bothAreMissing = leftIsNull && rightIsNull;

            if (bothAreMissing || leftIsNull != rightIsNull)
            {
                return bothAreMissing;
            }

            if (left.Fields.Count != right.Fields.Count)
            {
                return false;
            }

            foreach (var field in left.Fields)
            {
                if (!right.Fields.Contains(field))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(Layer left, Layer right)
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

        public static bool operator !=(Layer left, Layer right)
        {
            return !(left == right);
        }
    }
}
