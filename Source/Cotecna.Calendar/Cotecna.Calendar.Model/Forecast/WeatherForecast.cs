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
        #region constants

        private const string FORECAST_URL_CITY_PARAMETER_KEY = "q";
        private const string FORECAST_URL_CITY_PARAMETER_VALUE = "Barcelona";
        private const string FORECAST_URL_LOGIN_PARAMETER_KEY = "appid";
        private const string FORECAST_URL_LOGIN_PARAMETER_VALUE = "ab00f1f217b4c97b2f2ecfab05190206";
        private const string FORECAST_URL_WEB_API = "https://api.openweathermap.org/data/2.5/forecast";
        private const string PROXY_URL = "XXX";
        private const string PROXY_USERNAME = "XXX";
        private const string PROXY_PASS = "XXX";

        #endregion

        #region constructors

        private WeatherForecast() { }

        #endregion

        #region methods

        internal static async Task<JsonValue> GetWeatherForecast()
        {
            //Uncomment this if in your side have a proxy       
           //HttpClient client = WeatherForecast.BuildHttpClientWithProxy();

            HttpClient client = new HttpClient();
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
            var builder = new UriBuilder(FORECAST_URL_WEB_API);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query[FORECAST_URL_CITY_PARAMETER_KEY] = FORECAST_URL_CITY_PARAMETER_VALUE;
            query[FORECAST_URL_LOGIN_PARAMETER_KEY] = FORECAST_URL_LOGIN_PARAMETER_VALUE;
            builder.Query = query.ToString();
            string url = builder.ToString();
            return new Uri(url);
        }

        private static HttpClient BuildHttpClientWithProxy()
        {
            var proxy = new WebProxy()
            {
                Address = new Uri(PROXY_URL),
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(
                userName: PROXY_USERNAME,
                password: PROXY_PASS)
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
