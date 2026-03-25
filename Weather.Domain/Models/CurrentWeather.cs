namespace Weather.Domain.Models
{
    public class CurrentWeather
    {
        public double TempC { get; set; }
        public WeatherCondition Condition { get; set; }
    }
}
