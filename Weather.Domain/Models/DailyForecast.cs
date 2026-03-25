namespace Weather.Domain.Models
{
    public class DailyForecast
    {
        public string Date { get; set; }
        public double MaxTempC { get; set; }
        public double MinTempC { get; set; }
        public WeatherCondition Condition { get; set; }
    }
}
