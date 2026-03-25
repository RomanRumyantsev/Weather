using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Weather.Domain.Models;
using Weather.Domain.Interfaces;
using Weather.Infrastructure.ExternalModels;

namespace Weather.Infrastructure.Services
{
    public class WeatherApiService : IWeatherService
    {
        private readonly string _apiKey;
        private const string BaseUrl = "http://api.weatherapi.com/v1/";

        public WeatherApiService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<WeatherForecast> GetMoscowForecastAsync()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            var client = new RestClient(BaseUrl);
            var request = new RestRequest("forecast.json");
            request.AddQueryParameter("key", _apiKey);
            request.AddQueryParameter("q", "55.75,37.62");
            request.AddQueryParameter("days", "3");
            request.AddQueryParameter("lang", "ru");

            var finalUrl = client.BuildUri(request);
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"API Error: {response.StatusCode}");
            }

            var rawData = JsonConvert.DeserializeObject<WeatherApiResponseDto>(response.Content);
            return MapToDomain(rawData);
        }

        internal WeatherForecast MapToDomain(WeatherApiResponseDto raw)
        {
            long currentEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return new WeatherForecast
            {
                Current = new CurrentWeather
                {
                    TempC = raw.Current.TempC,
                    Condition = new WeatherCondition { Text = raw.Current.Condition.Text, Icon = raw.Current.Condition.Icon }
                },
                Daily = raw.Forecast.ForecastDays.Select(d => new DailyForecast
                {
                    Date = d.Date,
                    MaxTempC = d.Day.MaxTempC,
                    MinTempC = d.Day.MinTempC,
                    Condition = new WeatherCondition { Text = d.Day.Condition.Text, Icon = d.Day.Condition.Icon }
                }).ToList(),
                Hourly = raw.Forecast.ForecastDays
                    .SelectMany(d => d.Hour)
                    .Where(h => h.TimeEpoch >= currentEpoch)
                    .Take(30)
                    .Select(h => new HourlyForecast
                    {
                        Time = h.Time,
                        TempC = h.TempC,
                        TimeEpoch = h.TimeEpoch,
                        Condition = new WeatherCondition { Text = h.Condition.Text, Icon = h.Condition.Icon }
                    }).ToList()
            };
        }
    }
}
