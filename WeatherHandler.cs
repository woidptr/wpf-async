using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherDashboard
{
    struct WeatherStruct
    {
        public WeatherMain main { get; set; }
        [JsonPropertyName("dt")]
        public long datetime { get; set; }
        public WeatherSys sys { get; set; }
        [JsonPropertyName("timezone")]
        public int timezoneOffset { get; set; }
        public string name { get; set; }
    }

    struct WeatherMain
    {
        public float temp { get; set; }
        [JsonPropertyName("feels_like")]
        public float feelsLike { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    struct WeatherSys
    {
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

    class WeatherHandler
    {
        private const string API_KEY = "d2f5cf6ce7a811cb427a742f356dac66";
        private const string baseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

        public static async Task<WeatherStruct> GetWeatherInfo(string city)
        {
            string url = string.Format(baseUrl, city, API_KEY);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch information");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            Trace.WriteLine(jsonResponse);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            WeatherStruct weather = await JsonSerializer.DeserializeAsync<WeatherStruct>(stream, options);

            return weather;
        }

        public static string ConvertTime(long datetime, int offset)
        {
            DateTimeOffset utcDateTime = DateTimeOffset.FromUnixTimeSeconds(datetime);

            TimeSpan utcOffset = TimeSpan.FromSeconds(offset);
            DateTimeOffset localDateTime = utcDateTime.ToOffset(utcOffset);

            string formattedTime = localDateTime.ToString("HH:mm:ss");

            return formattedTime;
        }
    }
}
