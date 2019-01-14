using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    public class Giphy
    {

        [JsonProperty("data")]
        public IList<Datum> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }
}
