using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoGoGiphy.Core.Models
{
    public class Images
    {

        [JsonProperty("fixed_height_still")]
        public FixedHeightStill FixedHeightStill { get; set; }

        [JsonProperty("original_still")]
        public OriginalStill OriginalStill { get; set; }

        [JsonProperty("fixed_width")]
        public FixedWidth FixedWidth { get; set; }

        [JsonProperty("fixed_height_small_still")]
        public FixedHeightSmallStill FixedHeightSmallStill { get; set; }

        [JsonProperty("fixed_height_downsampled")]
        public FixedHeightDownsampled FixedHeightDownsampled { get; set; }

        [JsonProperty("preview")]
        public Preview Preview { get; set; }

        [JsonProperty("fixed_height_small")]
        public FixedHeightSmall FixedHeightSmall { get; set; }

        [JsonProperty("downsized_still")]
        public DownsizedStill DownsizedStill { get; set; }

        [JsonProperty("downsized")]
        public Downsized Downsized { get; set; }

        [JsonProperty("downsized_large")]
        public DownsizedLarge DownsizedLarge { get; set; }

        [JsonProperty("fixed_width_small_still")]
        public FixedWidthSmallStill FixedWidthSmallStill { get; set; }

        [JsonProperty("preview_webp")]
        public PreviewWebp PreviewWebp { get; set; }

        [JsonProperty("fixed_width_still")]
        public FixedWidthStill FixedWidthStill { get; set; }

        [JsonProperty("fixed_width_small")]
        public FixedWidthSmall FixedWidthSmall { get; set; }

        [JsonProperty("downsized_small")]
        public DownsizedSmall DownsizedSmall { get; set; }

        [JsonProperty("fixed_width_downsampled")]
        public FixedWidthDownsampled FixedWidthDownsampled { get; set; }

        [JsonProperty("downsized_medium")]
        public DownsizedMedium DownsizedMedium { get; set; }

        [JsonProperty("original")]
        public Original Original { get; set; }

        [JsonProperty("fixed_height")]
        public FixedHeight FixedHeight { get; set; }

        [JsonProperty("looping")]
        public Looping Looping { get; set; }

        [JsonProperty("original_mp4")]
        public OriginalMp4 OriginalMp4 { get; set; }

        [JsonProperty("preview_gif")]
        public PreviewGif PreviewGif { get; set; }

        [JsonProperty("480w_still")]
        public wStill wStill { get; set; }
    }
}
