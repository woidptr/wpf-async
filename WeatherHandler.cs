using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherDashboard
{
    struct WeatherInfo
    {
        public float temp { get; set; }
        public int humidity { get; set; }
    }

    class WeatherHandler
    {
        private const string API_KEY = "d2f5cf6ce7a811cb427a742f356dac66";
        private const string baseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

        public static async Task<WeatherInfo> GetWeatherInfo(string city)
        {
            HttpClient client = new HttpClient();

            string url = string.Format(baseUrl, city, API_KEY);

            HttpResponseMessage response = await client.GetAsync(url);

            string jsonResponse = await response.Content.ReadAsStringAsync();

            WeatherInfo weatherInfo = JsonSerializer.Deserialize<WeatherInfo>(jsonResponse);

            return weatherInfo;
        }
    }
}
