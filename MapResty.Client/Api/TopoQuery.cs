using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace MapResty.Client.Api
{
    public class TopoQuery : Client
    {
        public TopoQuery()
            : this("localhost", 11215)
        { }

        public TopoQuery(string host, int port)
        {
            var builder = new UriBuilder("http", host, port, "/rest/geometry");
            this.BaseUrl = builder.Uri;
        }

        public int[] Relate(IGeometryObject source, IGeometryObject[] targets, int relation)
        {
            var request = new RestRequest();
            request.Resource = "relation";
            request.Method = Method.POST;

            request.AddParameter("source", JsonConvert.SerializeObject(source));
            request.AddParameter("targets", JsonConvert.SerializeObject(targets));
            request.AddParameter("relation", relation);

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var array = JsonConvert.DeserializeObject<int[]>(data);
            return array;
        }

        public IGeometryObject[] Intersection(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/intersection", source, targets);
        }

        public IGeometryObject[] Simplify(IGeometryObject[] input)
        {
            return this.Topo("simplify", input);
        }

        public IGeometryObject[] Buffer(IGeometryObject[] input, double distance)
        {
            var request = new RestRequest();
            request.AddParameter("distance", distance);

            return this.Topo("analysis/buffer", input, request);
        }

        public IGeometryObject[] ConvexHull(IGeometryObject[] input)
        {
            return this.Topo("analysis/convexhull", input);
        }

        public IGeometryObject[] Difference(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/difference", source, targets);
        }

        public IGeometryObject[] SymDifference(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/symdifference", source, targets);
        }

        public IGeometryObject[] Union(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/union", source, targets);
        }

        private IGeometryObject[] Topo(string resourceUrl, IGeometryObject[] input)
        {
            return this.Topo(resourceUrl, null, input);
        }

        private IGeometryObject[] Topo(string resourceUrl, IGeometryObject[] input, RestRequest request)
        {
            return this.Topo(resourceUrl, null, input, request);
        }

        private IGeometryObject[] Topo(string resourceUrl, IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo(resourceUrl, source, targets, null);
        }

        private IGeometryObject[] Topo(string resourceUrl, IGeometryObject source, IGeometryObject[] targets, RestRequest request)
        {
            if (request == null)
            {
                request = new RestRequest();
            }

            request.Resource = resourceUrl;
            request.Method = Method.POST;

            if (source != null)
            {
                request.AddParameter("source", JsonConvert.SerializeObject(source));
            }
            request.AddParameter("targets", JsonConvert.SerializeObject(targets));

            var result = this.Execute(request);
            var data = Convert.ToString(result.Data);
            var geometries = JsonConvert.DeserializeObject<IGeometryObject[]>(data, converter);
            return geometries;
        }

        private static GeometryConverter converter = new GeometryConverter();
    }
}
