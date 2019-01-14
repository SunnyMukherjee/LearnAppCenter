using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    public class Meta
    {

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("response_id")]
        public string ResponseId { get; set; }
    }
}
