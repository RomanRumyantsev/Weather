using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Weather.Infrastructure.Services;
using Weather.Infrastructure.ExternalModels;
using Weather.Domain.Models;

namespace Weather.Tests
{
    [TestClass]
    public class WeatherServiceTests
    {
        private WeatherApiService _service;

        [TestInitialize]
        public void Setup()
        {            
            _service = new WeatherApiService("test_key");// Для тестов маппинга ключ не важен
        }

        [TestMethod]
        public void MapToDomain_ShouldFilterPastHours_KeepOnlyFuture()
        {            
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var rawData = CreateMockResponse(new List<long> { now - 3600, now + 3600 }); // Один час в прошлом, один в будущем
            var result = InvokeMapToDomain(rawData);
            Assert.AreEqual(1, result.Hourly.Count, "Должны остаться только будущие часы");  // Должен остаться только 1 час 
            Assert.IsTrue(result.Hourly.All(h => h.TimeEpoch >= now));
        }

        [TestMethod]
        public void MapToDomain_ShouldLimitHourlyForecast_ToMax30Items()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timestamps = Enumerable.Range(1, 50).Select(i => now + (i * 3600)).ToList();//50 часов в будущем
            var rawData = CreateMockResponse(timestamps);
            var result = InvokeMapToDomain(rawData);
            Assert.AreEqual(30, result.Hourly.Count, "Почасовой прогноз должен быть ограничен 30 элементами");
        }

        [TestMethod]
        public void MapToDomain_WhenNoFutureHours_ShouldReturnEmptyList()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var rawData = CreateMockResponse(new List<long> { now - 7200, now - 3600 });
            var result = InvokeMapToDomain(rawData);
            Assert.AreEqual(0, result.Hourly.Count, "Если все часы в прошлом, список должен быть пуст");
        }

        [TestMethod]
        public void MapToDomain_WithNullData_ShouldThrowException()
        {
            Assert.ThrowsException<NullReferenceException>(() => _service.MapToDomain(null));
        }

        // Метод для создания мока ответа API
        private WeatherApiResponseDto CreateMockResponse(List<long> hourEpochs)
        {
            return new WeatherApiResponseDto
            {
                Current = new CurrentDto { TempC = 10, Condition = new ConditionDto { Text = "Cloudy", Icon = "//cdn" } },
                Forecast = new ForecastDto
                {
                    ForecastDays = new List<ForecastDayDto>
                    {
                        new ForecastDayDto
                        {
                            Date = "2026-03-24",
                            Day = new DayDto { MaxTempC = 15, MinTempC = 5, Condition = new ConditionDto { Text = "Sunny" } },
                            Hour = hourEpochs.Select(e => new HourDto
                            {
                                TimeEpoch = e,
                                TempC = 12,
                                Time = "2026-03-24 12:00",
                                Condition = new ConditionDto { Text = "Clear" }
                            }).ToList()
                        }
                    }
                }
            };
        }

        private WeatherForecast InvokeMapToDomain(WeatherApiResponseDto dto)
        {
            var method = typeof(WeatherApiService)
                .GetMethod("MapToDomain", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            return (WeatherForecast)method.Invoke(_service, new[] { dto });
        }
    }
}
