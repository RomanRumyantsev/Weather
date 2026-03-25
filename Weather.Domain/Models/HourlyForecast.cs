namespace Weather.Domain.Models
{
    public class HourlyForecast
    {
        public long TimeEpoch { get; set; }
        public string Time { get; set; }
        public double TempC { get; set; }
        public WeatherCondition Condition { get; set; }
    }
}
