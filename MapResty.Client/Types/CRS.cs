using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapResty.Client.Types
{
    public class CRS
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "properties", Required = Required.Always)]
        public Dictionary<string, object> Properties { get; set; }

        public static CRS CreateProj4(string proj4str)
        {
            return new CRS
            {
                Type = "proj4",
                Properties = new Dictionary<string, object>
                {
                    { "proj", proj4str }
                }
            };
        }

        public static CRS EPSG4326 = CreateProj4("+proj=longlat +datum=WGS84 +no_defs");
        public static CRS EPSG3857 = CreateProj4("+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext +no_defs");
        public static CRS BD09LL = CreateProj4("+proj=longlat +datum=BD09");
        public static CRS GCJ02 = CreateProj4("+proj=longlat +datum=GCJ02");
        public static CRS WGS84 = EPSG4326;
    }
}
