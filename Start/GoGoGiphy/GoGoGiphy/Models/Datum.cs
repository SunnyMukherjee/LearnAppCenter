using Newtonsoft.Json;

namespace GoGoGiphy.Core.Models
{
    public class Datum
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("bitly_gif_url")]
        public string BitlyGifUrl { get; set; }

        [JsonProperty("bitly_url")]
        public string BitlyUrl { get; set; }

        [JsonProperty("embed_url")]
        public string EmbedUrl { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("content_url")]
        public string ContentUrl { get; set; }

        [JsonProperty("source_tld")]
        public string SourceTld { get; set; }

        [JsonProperty("source_post_url")]
        public string SourcePostUrl { get; set; }

        [JsonProperty("is_sticker")]
        public int IsSticker { get; set; }

        [JsonProperty("import_datetime")]
        public string ImportDatetime { get; set; }

        [JsonProperty("trending_datetime")]
        public string TrendingDatetime { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }


        //private ICommand _tappedCommand;

        //public ICommand TappedCommand
        //{
        //    get
        //    {
        //        return _tappedCommand ??
        //            (
        //                _tappedCommand = new Command<string>
        //                (
        //                    (obj) =>
        //                    {


        //                    }
        //                )
        //            );
        //    }
        //}       
    }
}
