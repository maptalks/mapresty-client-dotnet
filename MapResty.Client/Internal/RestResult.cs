using Newtonsoft.Json;

namespace MapResty.Client.Internal
{
    class RestResult
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        [JsonProperty(PropertyName = "errCode")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string ErrorMsg { get; set; }
    }
}
