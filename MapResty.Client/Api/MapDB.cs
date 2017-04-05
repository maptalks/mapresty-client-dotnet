using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using GeoJSON.Net.Feature;
using MapResty.Client.Types;
using MapResty.Client.Internal;

namespace MapResty.Client.Api
{
    public class MapDB : Client
    {
        public MapDB(string db)
            : this("localhost", 11215, db)
        { }

        public MapDB(string host, int port, string db)
        {
            var builder = new UriBuilder("http", host, port, "/rest/sdb/database/" + db);
            this.BaseUrl = builder.Uri;
        }

        public void Install(DbSettings settings)
        {
            var request = new RestRequest();
            request.Method = Method.POST;

            request.AddQueryParameter("op", "install");
            request.AddParameter("settings", JsonConvert.SerializeObject(settings));

            this.Execute(request);
        }

        public DbInfo GetDbInfo()
        {
            var request = new RestRequest();
            request.Method = Method.GET;

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var info = JsonConvert.DeserializeObject<DbInfo>(data);
            return info;
        }

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

        public void AddLayer(Layer layer)
        {
            var request = new RestRequest();
            request.Resource = "layers";
            request.Method = Method.POST;

            request.AddQueryParameter("op", "create");
            request.AddParameter("data", JsonConvert.SerializeObject(layer));

            this.Execute(request);
        }

        public void RemoveLayer(string id)
        {
            var request = new RestRequest();
            request.Resource = "layers";
            request.Method = Method.POST;

            request.AddQueryParameter("op", "remove");
            request.AddParameter("data", id);

            this.Execute(request);
        }

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

        public string QueryJSON(QueryFilter filter, int page, int count, string layerId)
        {
            return this.QueryJSON(filter, page, count, new string[] { layerId });
        }

        public List<FeatureCollection> Query(QueryFilter filter, int page, int count, string[] layerIds)
        {
            var json = this.QueryJSON(filter, page, count, layerIds);
            var result = JsonConvert.DeserializeObject<List<FeatureCollection>>(json);
            return result;
        }

        public List<FeatureCollection> Query(QueryFilter filter, int page, int count, string layerId)
        {
            return this.Query(filter, page, count, new string[] { layerId });
        }
    }
}
