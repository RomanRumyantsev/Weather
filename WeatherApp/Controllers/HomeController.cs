using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Weather.Domain.Interfaces;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        public ActionResult Index() => View();

        [HttpGet]
        public async Task<ActionResult> GetWeatherData()
        {
            try
            {
                var data = await _weatherService.GetMoscowForecastAsync();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
