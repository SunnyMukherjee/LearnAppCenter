using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    public class Pagination
    {

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}
