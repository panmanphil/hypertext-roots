using System.Collections.Generic;
using Newtonsoft.Json;

namespace HypertextRoots.HAL
{
    public abstract class LinkedResource
    {
        public LinkedResource()
        {
            this.Links = new Dictionary<string, Link>();
        }

        [JsonProperty("_links")]
        public IDictionary<string, Link> Links { get; private set; }

               
    }
}
