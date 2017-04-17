using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace MapResty.Client.Api
{
    /// <summary>
    /// 拓扑计算相关API
    /// </summary>
    public class TopoQuery : Client
    {
        /// <summary>
        /// 构造函数，使用默认的host("localhost")与port(11215)
        /// </summary>
        public TopoQuery()
            : this("localhost", 11215)
        { }

        /// <summary>
        /// 构造函数，使用指定的host与port
        /// </summary>
        /// <param name="host">服务器主机</param>
        /// <param name="port">服务器端口</param>
        public TopoQuery(string host, int port)
        {
            var builder = new UriBuilder("http", host, port, "/rest/geometry");
            this.BaseUrl = builder.Uri;
        }

        /// <summary>
        /// 计算source与每个target是否满足指定空间关系
        /// </summary>
        /// <param name="source">源Geometry</param>
        /// <param name="targets">目标Geometry数组</param>
        /// <param name="relation">空间关系(见SpatialFilter)</param>
        /// <returns>一个数组，元素值为1表示source与target符合指定空间关系，为0则不满足</returns>
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

        /// <summary>
        /// 计算source与每个target的交集
        /// </summary>
        /// <param name="source">源Geometry</param>
        /// <param name="targets">目标Geometry数组</param>
        /// <returns>与targets同样长度的数组，每个元素表示source与相应target的交集</returns>
        public IGeometryObject[] Intersection(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/intersection", source, targets);
        }

        /// <summary>
        /// 简化输入的Geometry数组
        /// </summary>
        /// <param name="input">待简化的Geometry数组</param>
        /// <returns>简化后的Geometry数组</returns>
        public IGeometryObject[] Simplify(IGeometryObject[] input)
        {
            return this.Topo("simplify", input);
        }

        /// <summary>
        /// 对输入的Geometry数组做缓冲计算
        /// </summary>
        /// <param name="input">待计算缓冲的Geometry数组</param>
        /// <param name="distance">缓冲距离(米)</param>
        /// <returns>做缓冲计算后的Geometry数组</returns>
        public IGeometryObject[] Buffer(IGeometryObject[] input, double distance)
        {
            var request = new RestRequest();
            request.AddParameter("distance", distance);

            return this.Topo("analysis/buffer", input, request);
        }

        /// <summary>
        /// 对输入的Geometry数组做凸包计算
        /// </summary>
        /// <param name="input">带计算凸包的Geometry数组</param>
        /// <returns>做凸包计算后的Geometry数组</returns>
        public IGeometryObject[] ConvexHull(IGeometryObject[] input)
        {
            return this.Topo("analysis/convexhull", input);
        }

        /// <summary>
        /// 计算source与每个target的Difference
        /// </summary>
        /// <param name="source">源Geometry</param>
        /// <param name="targets">目标Geometry数组</param>
        /// <returns>与targets同样长度的数组，每个元素表示source与相应target的Difference</returns>
        public IGeometryObject[] Difference(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/difference", source, targets);
        }

        /// <summary>
        /// 计算source与每个target的SymDifference
        /// </summary>
        /// <param name="source">源Geometry</param>
        /// <param name="targets">目标Geometry数组</param>
        /// <returns>与targets同样长度的数组，每个元素表示source与相应target的SymDifference</returns>
        public IGeometryObject[] SymDifference(IGeometryObject source, IGeometryObject[] targets)
        {
            return this.Topo("analysis/symdifference", source, targets);
        }

        /// <summary>
        /// 计算source与每个target的并集
        /// </summary>
        /// <param name="source">源Geometry</param>
        /// <param name="targets">目标Geometry数组</param>
        /// <returns>与targets同样长度的数组，每个元素表示source与相应target的并集</returns>
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
