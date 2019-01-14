using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    [Table("GifDataItem")]
    public class GifDataItem
    {
        [PrimaryKey]
        public string Id { get; set; }

        [Indexed]
        public int GifCollectionId { get; set; }

        public string CachedImageAutomationId { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string EmbedUrl { get; set; }

        public string Source { get; set; }

        public string Rating { get; set; }

        public string ContentUrl { get; set; }

        public string TrendingDatetime { get; set; }

        public string Title { get; set; }

        public string ImagePreviewGifUrl { get; set; }

        public string ImageDownsizedUrl { get; set; }
    }
}
