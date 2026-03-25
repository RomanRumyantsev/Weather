using Autofac;
using Autofac.Integration.Mvc;
using System.Configuration;
using System.Web.Mvc;
using Weather.Domain.Interfaces;
using Weather.Infrastructure.Services;

namespace WeatherApp
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            string apiKey = ConfigurationManager.AppSettings["WeatherApiKey"];
            builder.Register(c => new WeatherApiService(apiKey))
                   .As<IWeatherService>()
                   .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
