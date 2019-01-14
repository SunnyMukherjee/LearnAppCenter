using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    public class User
    {

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("banner_url")]
        public string BannerUrl { get; set; }

        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }
    }
}
