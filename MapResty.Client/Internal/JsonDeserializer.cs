using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace MapResty.Client.Internal
{
    class JsonDeserializer : IDeserializer
    {
        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
