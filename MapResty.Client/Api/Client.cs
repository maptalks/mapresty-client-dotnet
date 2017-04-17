using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using MapResty.Client.Internal;

namespace MapResty.Client.Api
{
    /// <summary>
    /// Rest客户端基类
    /// </summary>
    public abstract class Client
    {
        internal Uri BaseUrl { get; set; }

        private bool compress = true;
        /// <summary>
        /// 指示服务端是否使用GZip传输数据
        /// </summary>
        public bool Compress
        {
            get { return compress; }
            set { compress = value; }
        }

        internal RestResult Execute(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            client.Encoding = Encoding.UTF8;
            // Replace default handler
            client.AddHandler("*", new JsonDeserializer());

            if (Compress)
            {
                request.AddHeader("Accept-Encoding", "gzip, deflate");
            }
            if (request.Method == Method.POST || request.Method == Method.PUT)
            {
                request.AddHeader("Accept-Charset", "UTF-8");
            }

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "RestSharp: Exception occured";
                throw new ApplicationException(message, response.ErrorException);
            }

            var result = JsonConvert.DeserializeObject<RestResult>(response.Content);
            if (result == null)
            {
                throw new SerializationException("Invalid JSON string");
            }
            if (!result.Success)
            {
                throw new RestException(result.ErrorCode, result.ErrorMsg);
            }

            return result;
        }
    }
}
