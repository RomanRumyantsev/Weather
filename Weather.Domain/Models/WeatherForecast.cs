using System.Collections.Generic;

namespace Weather.Domain.Models
{
    public class WeatherForecast
    {
        public CurrentWeather Current { get; set; }
        public List<DailyForecast> Daily { get; set; }
        public List<HourlyForecast> Hourly { get; set; }
    }
}
