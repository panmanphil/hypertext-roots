using Newtonsoft.Json;

namespace HypertextRoots.HAL
{
    public class Link
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string rel { get; set; }
        
        public string href { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool templated { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string deprecation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string profile { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hreflang { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }
    }

    
}
