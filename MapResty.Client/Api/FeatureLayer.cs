using System;
using System.Collections.Generic;
using MapResty.Client.Types;
using RestSharp;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using MapResty.Client.Internal;

namespace MapResty.Client.Api
{
    /// <summary>
    /// 要素图层相关API
    /// </summary>
    public class FeatureLayer : Client
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">图层ID</param>
        /// <param name="db">MapDB实例</param>
        public FeatureLayer(string id, MapDB db)
        {
            this.id = id;
            this.db = db;
            this.BaseUrl = this.db.BaseUrl;
        }

        /// <summary>
        /// 增加图层表字段
        /// </summary>
        /// <param name="field">图层表字段</param>
        public void AddLayerField(LayerField field)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/fields";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "create");
            request.AddParameter("data", JsonConvert.SerializeObject(field));

            this.Execute(request);
        }

        /// <summary>
        /// 更新图层表字段
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="field">新的图层表字段</param>
        public void UpdateLayerField(string fieldName, LayerField field)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/fields/{name}";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddUrlSegment("name", fieldName);
            request.AddQueryParameter("op", "update");
            request.AddParameter("data", JsonConvert.SerializeObject(field));

            this.Execute(request);
        }

        /// <summary>
        /// 删除图层表字段
        /// </summary>
        /// <param name="fieldName">字段名</param>
        public void RemoveLayerField(string fieldName)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/fields/{name}";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddUrlSegment("name", fieldName);
            request.AddQueryParameter("op", "remove");

            this.Execute(request);
        }

        /// <summary>
        /// 获取所有图层表字段
        /// </summary>
        /// <returns>图层表字段列表</returns>
        public List<LayerField> GetLayerFields()
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/fields";
            request.Method = Method.GET;

            request.AddUrlSegment("id", this.id);

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var fields = JsonConvert.DeserializeObject<List<LayerField>>(data);
            return fields;
        }

        private void Add(string json, CRS crs)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "create");
            request.AddParameter("data", json);
            if (crs != null)
            {
                request.AddParameter("crs", JsonConvert.SerializeObject(crs));
            }

            this.Execute(request);
        }

        /// <summary>
        /// 插入要素数据
        /// </summary>
        /// <param name="feature">要素数据，GeoJSON编码格式</param>
        /// <param name="crs">数据的坐标参考系</param>
        public void Add(Feature feature, CRS crs = null)
        {
            var data = JsonConvert.SerializeObject(feature);
            this.Add(data, crs);
        }

        /// <summary>
        /// 插入多个要素数据
        /// </summary>
        /// <param name="features">要素数据数组</param>
        /// <param name="crs">数据的坐标参考系</param>
        public void Add(Feature[] features, CRS crs = null)
        {
            var data = JsonConvert.SerializeObject(features);
            this.Add(data, crs);
        }

        /// <summary>
        /// 插入多个要素数据
        /// </summary>
        /// <param name="features">要素数据列表</param>
        /// <param name="crs">数据的坐标参考系</param>
        public void Add(List<Feature> features, CRS crs = null)
        {
            var data = JsonConvert.SerializeObject(features);
            this.Add(data, crs);
        }

        /// <summary>
        /// 查询符合指定条件的第一个要素数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>符合查询条件的第一个要素数据，或null</returns>
        public Feature GetFirst(string condition)
        {
            var filter = new QueryFilter();
            filter.Condition = condition;
            var features = this.Query(filter, 0, 1);
            if (features != null && features.Count > 0)
            {
                return features[0];
            }
            return null;
        }

        public void Update(string condition, Feature feature, CRS crs)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "update");
            request.AddParameter("data", JsonConvert.SerializeObject(feature));
            if (crs != null)
            {
                request.AddParameter("crs", JsonConvert.SerializeObject(crs));
            }
            request.AddParameter("condition", condition);

            this.Execute(request);
        }

        /// <summary>
        /// 删除符合指定查询条件的要素数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        public void Remove(string condition)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "remove");
            request.AddParameter("condition", condition);

            this.Execute(request);
        }

        /// <summary>
        /// 删除所有要素数据
        /// </summary>
        public void Remove()
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "removeAll");

            this.Execute(request);
        }

        /// <summary>
        /// 删除所有要素数据，同Remove()
        /// </summary>
        public void Clear()
        {
            this.Remove();
        }

        /// <summary>
        /// 更新符合查询条件的要素数据的属性
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="properties">新的属性</param>
        public void UpdateProperties(string condition, object properties)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "updateProperties");
            request.AddParameter("condition", condition);
            request.AddParameter("data", JsonConvert.SerializeObject(properties));

            this.Execute(request);
        }

        /// <summary>
        /// 用指定参数查询此空间图层
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <param name="page">页数</param>
        /// <param name="count">每页大小</param>
        /// <returns>符合条件的要素数据列表，或null</returns>
        public List<Feature> Query(QueryFilter filter, int page, int count)
        {
            var matrix = this.db.Query(filter, page, count, this.id);
            if (matrix.Count == 0)
            {
                return null;
            }
            return matrix[0].Features;
        }

        /// <summary>
        /// 用指定参数查询此空间图层
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <param name="page">页数</param>
        /// <param name="count">每页大小</param>
        /// <returns>JSON字符串表示的符合条件的查询结果</returns>
        public string QueryJSON(QueryFilter filter, int page, int count)
        {
            return this.db.QueryJSON(filter, page, count, this.id);
        }

        /// <summary>
        /// 查询符合查询过滤器的要素数据的数量
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <returns>符合查询过滤器的要素数据的数量</returns>
        public long Count(QueryFilter filter)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "count");
            request.AddParametersFromQueryFilter(filter);

            var result = this.Execute(request);
            long count = Convert.ToInt64(result.Data);
            return count;
        }

        private string id;
        private MapDB db;
    }
}
