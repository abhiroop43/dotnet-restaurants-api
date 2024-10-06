namespace Restaurants.API.Services.Interfaces;

public interface IWeatherForecastService
{
    /**
     * Get a list of weather forecasts.
     * @param resultsRequested The number of weather forecasts to return.
     * @param minTemp The minimum temperature for the weather forecasts.
     * @param maxTemp The maximum temperature for the weather forecasts.
     * @return A list of weather forecasts.
     */
    IEnumerable<WeatherForecast> GetWeatherForecasts(int resultsRequested, int minTemp, int maxTemp);
}