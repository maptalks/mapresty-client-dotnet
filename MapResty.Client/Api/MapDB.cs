using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using GeoJSON.Net.Feature;
using MapResty.Client.Types;
using MapResty.Client.Internal;

namespace MapResty.Client.Api
{
    /// <summary>
    /// MapDB相关API
    /// </summary>
    public class MapDB : Client
    {
        /// <summary>
        /// 构造函数，使用默认的host("localhost")与port(11215)
        /// </summary>
        /// <param name="db">空间数据库名字</param>
        public MapDB(string db)
            : this("localhost", 11215, db)
        { }

        /// <summary>
        /// 构造函数，使用指定host与port
        /// </summary>
        /// <param name="host">服务器主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="db">空间数据库名字</param>
        public MapDB(string host, int port, string db)
        {
            var builder = new UriBuilder("http", host, port, "/rest/sdb/database/" + db);
            this.BaseUrl = builder.Uri;
        }

        /// <summary>
        /// 初始化空间数据库
        /// </summary>
        /// <param name="settings">初始化参数</param>
        public void Install(DbSettings settings)
        {
            var request = new RestRequest();
            request.Method = Method.POST;

            request.AddQueryParameter("op", "install");
            request.AddParameter("settings", JsonConvert.SerializeObject(settings));

            this.Execute(request);
        }

        /// <summary>
        /// 获取空间数据库信息
        /// </summary>
        /// <returns>空间数据库信息</returns>
        public DbInfo GetDbInfo()
        {
            var request = new RestRequest();
            request.Method = Method.GET;

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var info = JsonConvert.DeserializeObject<DbInfo>(data);
            return info;
        }

        /// <summary>
        /// 获取所有图层信息
        /// </summary>
        /// <returns>图层信息列表</returns>
        public List<Layer> GetAllLayers()
        {
            var request = new RestRequest();
            request.Resource = "layers";
            request.Method = Method.GET;

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var layers = JsonConvert.DeserializeObject<List<Layer>>(data);
            return layers;
        }

        /// <summary>
        /// 获取指定图层信息
        /// </summary>
        /// <param name="id">图层ID</param>
        /// <returns>图层信息</returns>
        public Layer GetLayer(string id)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}";
            request.Method = Method.GET;

            request.AddUrlSegment("id", id);

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var layer = JsonConvert.DeserializeObject<Layer>(data);
            return layer;
        }

        /// <summary>
        /// 增加图层
        /// </summary>
        /// <param name="layer">图层信息</param>
        public void AddLayer(Layer layer)
        {
            var request = new RestRequest();
            request.Resource = "layers";
            request.Method = Method.POST;

            request.AddQueryParameter("op", "create");
            request.AddParameter("data", JsonConvert.SerializeObject(layer));

            this.Execute(request);
        }

        /// <summary>
        /// 删除图层
        /// </summary>
        /// <param name="id">图层ID</param>
        public void RemoveLayer(string id)
        {
            var request = new RestRequest();
            request.Resource = "layers";
            request.Method = Method.POST;

            request.AddQueryParameter("op", "remove");
            request.AddParameter("data", id);

            this.Execute(request);
        }

        /// <summary>
        /// 更新指定图层
        /// </summary>
        /// <param name="id">要更新的图层ID</param>
        /// <param name="layer">新的图层信息</param>
        public void UpdateLayer(string id, Layer layer)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}";
            request.Method = Method.POST;

            request.AddUrlSegment("id", id);
            request.AddQueryParameter("op", "update");
            request.AddParameter("data", JsonConvert.SerializeObject(layer));

            this.Execute(request);
        }

        /// <summary>
        /// 用指定参数查询空间数据库
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <param name="page">页数</param>
        /// <param name="count">每页大小</param>
        /// <param name="layerIds">图层ID数组</param>
        /// <returns>JSON字符串表示的查询结果</returns>
        public string QueryJSON(QueryFilter filter, int page, int count, string[] layerIds)
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", String.Join(",", layerIds));
            request.AddQueryParameter("op", "query");
            request.AddParametersFromQueryFilter(filter);
            request.AddParameter("page", page);
            request.AddParameter("count", count);

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            return data;
        }

        /// <summary>
        /// 用指定参数查询空间数据库
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <param name="page">页数</param>
        /// <param name="count">每页大小</param>
        /// <param name="layerId">图层ID</param>
        /// <returns>JSON字符串表示的查询结果</returns>
        public string QueryJSON(QueryFilter filter, int page, int count, string layerId)
        {
            return this.QueryJSON(filter, page, count, new string[] { layerId });
        }

        /// <summary>
        /// 用指定参数查询空间数据库
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <param name="page">页数</param>
        /// <param name="count">每页大小</param>
        /// <param name="layerIds">图层ID数组</param>
        /// <returns>GeoJSON FeatureCollection的列表</returns>
        public List<FeatureCollection> Query(QueryFilter filter, int page, int count, string[] layerIds)
        {
            var json = this.QueryJSON(filter, page, count, layerIds);
            var result = JsonConvert.DeserializeObject<List<FeatureCollection>>(json);
            return result;
        }

        /// <summary>
        /// 用指定参数查询空间数据库
        /// </summary>
        /// <param name="filter">查询过滤器</param>
        /// <param name="page">页数</param>
        /// <param name="count">每页大小</param>
        /// <param name="layerId">图层ID</param>
        /// <returns>GeoJSON FeatureCollection的列表</returns>
        public List<FeatureCollection> Query(QueryFilter filter, int page, int count, string layerId)
        {
            return this.Query(filter, page, count, new string[] { layerId });
        }
    }
}
