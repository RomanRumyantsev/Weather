# Weather Web Application (.NET Framework)

Тестовое задание: Погодное веб-приложение для города Москва с использованием Clean Architecture.

## Архитектура
Приложение спроектировано по принципам **Clean Architecture**, что обеспечивает слабую связанность слоев и высокую тестируемость:

1.  **Weather.Domain**: Сердце приложения. Содержит интерфейсы (`IWeatherService`) и чистые доменные модели. Не имеет внешних зависимостей.
2.  **Weather.Infrastructure**: Слой реализации. Содержит логику взаимодействия с внешним API (WeatherAPI) через `RestSharp`, DTO-модели и логику маппинга/фильтрации данных.
3.  **WeatherApp (Web)**: Слой представления (ASP.NET MVC 5). Отвечает за UI, маршрутизацию и конфигурацию DI.
4.  **Weather.Tests**: Проект с Unit-тестами (MSTest + Moq) для проверки бизнес-логики маппинга и фильтрации.

## Технологический стек
*   **Backend**: .NET Framework 4.8, ASP.NET MVC 5, Web API 2.
*   **DI Container**: Autofac (Constructor Injection).
*   **API Client**: RestSharp.
*   **JSON Serialization**: Newtonsoft.Json.
*   **Frontend**: Bootstrap 5, jQuery (модульный JS с паттерном State Management).
*   **Testing**: MSTest.

## Особенности реализации
*   **State Management**: На фронтенде реализовано четкое переключение состояний: `Loading` -> `Success` / `Error`.
*   **Retry Logic**: Наличие кнопки "Повторить запрос" при возникновении ошибок сети или API.
*   **Data Transformation**: Логика фильтрации почасового прогноза (текущий день + следующий) вынесена на сторону сервера (Infrastructure Layer).
*   **Security**: API-ключ вынесен в `Web.config` и пробрасывается в сервис через DI.
*   **Performance**: Использование асинхронных запросов (`async/await`) на всех уровнях.

## Запуск проекта
1.  Откройте файл решения `.sln` в Visual Studio 2022.
2.  Убедитесь, что в `Web.config` проекта **WeatherApp** указан валидный `WeatherApiKey`.
3.  Восстановите NuGet пакеты (Restore NuGet Packages).
4.  Установите **WeatherApp** как Startup Project.
5.  Нажмите `F5` для запуска.

## Тестирование
Для запуска тестов перейдите в `Test Explorer` (Тест -> Обозреватель тестов) и нажмите **Run All**. Тесты покрывают логику фильтрации временных интервалов и корректность преобразования данных из внешнего API.
