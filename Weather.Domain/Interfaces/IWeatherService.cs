using System.Threading.Tasks;
using Weather.Domain.Models;

namespace Weather.Domain.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetMoscowForecastAsync();
    }
}
