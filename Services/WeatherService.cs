using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services
{
    class WeatherService
    {
        private readonly HttpClient _client;
        private const string API_KEY = "d2f5cf6ce7a811cb427a742f356dac66";
        private const string baseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

        public WeatherService()
        {
            this._client = new HttpClient();
        }

        public async Task<WeatherModel> GetWeatherInfo(string city)
        {
            string url = string.Format(baseUrl, city, API_KEY);

            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch information");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Trace.WriteLine(jsonResponse);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            WeatherModel weather = await JsonSerializer.DeserializeAsync<WeatherModel>(stream, options);

            return weather;
        }

        // cpu-bound operations, have to by sync
        public string ConvertTime(long datetime, int offset)
        {
            DateTimeOffset utcDateTime = DateTimeOffset.FromUnixTimeSeconds(datetime);

            TimeSpan utcOffset = TimeSpan.FromSeconds(offset);
            DateTimeOffset localDateTime = utcDateTime.ToOffset(utcOffset);

            string formattedTime = localDateTime.ToString("HH:mm");

            return formattedTime;
        }
    }
}
