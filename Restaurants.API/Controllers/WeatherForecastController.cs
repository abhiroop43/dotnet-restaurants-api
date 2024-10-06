using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Dto;
using Restaurants.API.Services.Interfaces;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet("{resultsRequested:int?}/{minTemp:int?}/{maxTemp:int?}")]
    public IActionResult Get([FromRoute] int resultsRequested = 5,
        [FromRoute] int minTemp = -20,
        [FromRoute] int maxTemp = 55)
    {
        return Ok(_weatherForecastService.GetWeatherForecasts(resultsRequested, minTemp, maxTemp));
    }

    [HttpPost("/generate")]
    public IActionResult Generate([FromBody] GenerateResultsRequest request)
    {
        try
        {
            return Ok(_weatherForecastService
                .GetWeatherForecasts(
                    request.ResultsCount,
                    request.MinTemp,
                    request.MaxTemp
                ));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating weather forecasts.");
            return StatusCode(500, "An error occurred while generating weather forecasts.");
        }
    }
}