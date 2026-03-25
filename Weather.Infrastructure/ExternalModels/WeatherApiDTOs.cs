using System.Collections.Generic;
using Newtonsoft.Json;

namespace Weather.Infrastructure.ExternalModels
{
    #region WeatherApiResponseDto
    internal class WeatherApiResponseDto
    {
        public CurrentDto Current { get; set; }
        public ForecastDto Forecast { get; set; }
    }
    #endregion

    #region CurrentDto
    internal class CurrentDto
    {
        [JsonProperty("temp_c")]
        public double TempC { get; set; }
        public ConditionDto Condition { get; set; }
    }
    #endregion

    #region ForecastDto
    internal class ForecastDto
    {
        [JsonProperty("forecastday")]
        public List<ForecastDayDto> ForecastDays { get; set; }
    }
    #endregion

    #region ForecastDayDto
    internal class ForecastDayDto
    {
        public string Date { get; set; }
        public DayDto Day { get; set; }
        public List<HourDto> Hour { get; set; }
    }
    #endregion

    #region DayDto
    internal class DayDto
    {
        [JsonProperty("maxtemp_c")]
        public double MaxTempC { get; set; }
        [JsonProperty("mintemp_c")]
        public double MinTempC { get; set; }
        public ConditionDto Condition { get; set; }
    }
    #endregion

    #region HourDto
    internal class HourDto
    {
        [JsonProperty("time_epoch")]
        public long TimeEpoch { get; set; }
        public string Time { get; set; }
        [JsonProperty("temp_c")]
        public double TempC { get; set; }
        public ConditionDto Condition { get; set; }
    }
    #endregion

    #region ConditionDto
    internal class ConditionDto
    {
        public string Text { get; set; }
        public string Icon { get; set; }
    }
    #endregion
}
