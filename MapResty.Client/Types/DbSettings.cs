using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    public class DbSettings
    {
        [JsonProperty(PropertyName = "crs")]
        public CRS CRS { get; set; }
    }
}
