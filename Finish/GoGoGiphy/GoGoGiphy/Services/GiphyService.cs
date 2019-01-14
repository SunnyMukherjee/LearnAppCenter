using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GoGoGiphy.Core.Models;
using Newtonsoft.Json;

namespace GoGoGiphy.Core.Services
{
    public class GiphyService
    {

        #region Members

        private HttpClient httpClient = new HttpClient();
        private string _trendingUrl;

        #endregion


        #region Commands



        #endregion


        #region Constructors

        public GiphyService()
        {
            Initialize();
        }

        #endregion


        #region Functions

        internal async Task<Giphy> GetTrendingGifs()
        {
            Giphy giphy = new Giphy();

            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(_trendingUrl);
                string jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                //giphy = await Task.Run(() => JsonConvert.DeserializeObject<Giphy>(jsonString));               
                giphy = JsonConvert.DeserializeObject<Giphy>(jsonString);

                Settings.Offset += Settings.LimitCount;
                _trendingUrl = String.Format(Settings.TrendingUrl, Settings.ApiKey, Settings.LimitCount, Settings.Offset, Settings.Rating);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return giphy;
        }

        private void Initialize()
        {
            _trendingUrl = String.Format(Settings.TrendingUrl, Settings.ApiKey, Settings.LimitCount, Settings.Offset, Settings.Rating);

            httpClient.BaseAddress = new Uri(Settings.TrendingUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        internal async Task<Giphy> SearchGifs(string searchText)
        {
            Giphy giphy = new Giphy();

            try
            {
                if (!String.IsNullOrEmpty(searchText))
                {
                    string searchUrl = String.Format(Settings.SearchUrl, Settings.ApiKey, searchText, Settings.LimitCount, Settings.Offset, Settings.Rating);

                    HttpResponseMessage responseMessage = await httpClient.GetAsync(searchUrl);
                    string jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    giphy = JsonConvert.DeserializeObject<Giphy>(jsonString);

                    Settings.Offset += Settings.LimitCount;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return giphy;
        }


        internal void TestSerialization()
        {
            Giphy giphy = new Giphy()
            {
                Data = new List<Datum>() { },
                Pagination = new Pagination(),
                Meta = new Meta()
            };

            string json = JsonConvert.SerializeObject(giphy);
        }

        #endregion
    }
}
