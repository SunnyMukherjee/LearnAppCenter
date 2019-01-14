using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class TestGoGoGiphy
    {
        private string searchUrl = @"https://api.giphy.com/v1/gifs/search?api_key={0}&q={1}&limit=25&offset=0&rating=G&lang=en";
        private string apiKey = "GIPHYAPIKEY";
        private string searchPhrase = "Iron Man";

        private static readonly HttpClient httpClient = new HttpClient();

        [SetUp]
        public void Initialize()
        {
            searchUrl = String.Format(searchUrl, apiKey, searchPhrase);
            httpClient.BaseAddress = new Uri(searchUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Test]
        public void TestSearchAsync()
        {
            HttpResponseMessage response = httpClient.GetAsync(searchUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content.ReadAsStringAsync().Result;
                Assert.Pass();
            }
        }
    }
}
