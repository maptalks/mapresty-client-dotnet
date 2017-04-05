using System;
using System.Collections.Generic;
using MapResty.Client.Types;
using RestSharp;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using MapResty.Client.Internal;

namespace MapResty.Client.Api
{
    public class FeatureLayer : Client
    {
        public FeatureLayer(string id, MapDB db)
        {
            this.id = id;
            this.db = db;
            this.BaseUrl = this.db.BaseUrl;
        }

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

        public void Add(Feature feature, CRS crs = null)
        {
            var data = JsonConvert.SerializeObject(feature);
            this.Add(data, crs);
        }

        public void Add(Feature[] features, CRS crs = null)
        {
            var data = JsonConvert.SerializeObject(features);
            this.Add(data, crs);
        }

        public void Add(List<Feature> features, CRS crs = null)
        {
            var data = JsonConvert.SerializeObject(features);
            this.Add(data, crs);
        }

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

        public void Remove()
        {
            var request = new RestRequest();
            request.Resource = "layers/{id}/data";
            request.Method = Method.POST;

            request.AddUrlSegment("id", this.id);
            request.AddQueryParameter("op", "removeAll");

            this.Execute(request);
        }

        public void Clear()
        {
            this.Remove();
        }

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

        public List<Feature> Query(QueryFilter filter, int page, int count)
        {
            var matrix = this.db.Query(filter, page, count, this.id);
            if (matrix.Count == 0)
            {
                return null;
            }
            return matrix[0].Features;
        }

        public string QueryJSON(QueryFilter filter, int page, int count)
        {
            return this.db.QueryJSON(filter, page, count, this.id);
        }

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
