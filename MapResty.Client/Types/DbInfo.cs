using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    public class DbInfo
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "crs", Required = Required.Always)]
        public CRS CRS { get; set; }
        
        [JsonProperty(PropertyName = "version", Required = Required.Always)]
        public string Version { get; set; }
    }
}
