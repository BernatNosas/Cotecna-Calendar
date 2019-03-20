using System;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Cotecna.Calendar.Model.Forecast
{
    internal class WeatherForecast
    {

        #region constructors

        private WeatherForecast() { }

        #endregion

        #region methods

        internal static async Task<JsonValue> GetWeatherForecast()
        {
            HttpClient client = new HttpClient();
            //HttpClient client = WeatherForecast.BuildHttpClientWithProxy();
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = WeatherForecast.GetUri(),
                Method = HttpMethod.Get,
            };
            JsonValue res = null;
            try
            {
                await client.SendAsync(request).
                ContinueWith((taskwithmsg) =>
                {
                    if (taskwithmsg.IsFaulted)
                    {
                        throw taskwithmsg.Exception;
                    }
                    HttpResponseMessage response = taskwithmsg.Result;
                    if (response.StatusCode != HttpStatusCode.OK) { throw new Exception(response.ReasonPhrase); }
                    var resTask = response.Content.ReadAsStringAsync();
                    resTask.Wait();
                    res = JsonObject.Parse(resTask.Result);
                });
                return res;
            }
            catch(Exception e)
            {
                return null;
            }                       
        }

        private static Uri GetUri()
        {
            var builder = new UriBuilder("https://api.openweathermap.org/data/2.5/forecast");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["q"] = "Barcelona";
            query["appid"] = "ab00f1f217b4c97b2f2ecfab05190206";
            builder.Query = query.ToString();
            string url = builder.ToString();
            return new Uri(url);
        }

        private static HttpClient BuildHttpClientWithProxy()
        {
            var proxy = new WebProxy()
            {
                Address = new Uri($"XXX"),
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(
                userName: "XXX",
                password: "XXX")
            };

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            return new HttpClient(handler: httpClientHandler, disposeHandler: true); 
        }
        #endregion
    }
}
