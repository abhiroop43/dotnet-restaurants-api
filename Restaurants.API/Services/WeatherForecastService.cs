using Restaurants.API.Services.Interfaces;

namespace Restaurants.API.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    /**
     * Get a list of weather forecasts.
     * @param resultsRequested The number of weather forecasts to return.
     * @param minTemp The minimum temperature for the weather forecasts.
     * @param maxTemp The maximum temperature for the weather forecasts.
     * @return A list of weather forecasts.
     */
    public IEnumerable<WeatherForecast> GetWeatherForecasts(int resultsRequested, int minTemp, int maxTemp)
    {
        if (resultsRequested < 1)
            throw new ArgumentException("The number of weather forecasts requested must be at least 1.");

        if (minTemp > maxTemp)
            throw new ArgumentException(
                "The minimum temperature must be less than or equal to the maximum temperature.");

        return Enumerable.Range(1, resultsRequested).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(minTemp, maxTemp),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}