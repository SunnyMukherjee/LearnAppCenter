using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GoGoGiphy.Core
{
    public class Settings
    {
        internal static string AppName { get; set; } = "GoGoGiphy";

        // TODO: Add Secret value for iOS app here.
        internal static string AppCenterSecretiOS { get; set; } = "iOSSecret";

        // TODO: Add Secret value for Android app here.
        internal static string AppCenterSecretAndroid { get; set; } = "AndroidSecret";

        internal static string SearchUrl { get; set; } = @"https://api.giphy.com/v1/gifs/search?api_key={0}&q={1}&limit={2}&offset={3}&rating={4}&lang=en";

        internal static string TrendingUrl { get; set; } = @"https://api.giphy.com/v1/gifs/trending?api_key={0}&limit={1}&offset={2}&rating={3}";

        // TODO: Add key for Giphy API here.
        internal static string ApiKey { get; } = "GiphyApiKey";

        internal static int LimitCount { get; set; } = 30;

        internal static int Offset { get; set; } = 0;

        private static string _rating;

        internal static string Rating
        {
            get
            {
                // Retrieve the rating from either the KeyStore or the KeyChain.
                string savedRating = SecureStorage.GetAsync(nameof(Rating)).Result;
                return (savedRating) ?? "G";
            }
            set
            {
                _rating = value;

                // Save the value to the KeyStore or the KeyChain.
                SecureStorage.SetAsync(nameof(Rating), _rating);
            }
        }
    }
}
