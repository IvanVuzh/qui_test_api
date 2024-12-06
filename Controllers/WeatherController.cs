using Microsoft.AspNetCore.Mvc;
using qui_test_api.WeatherApiIntegration.OpenWeatherApi;

namespace qui_test_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly OpenWeatherApiService _weatherService;

        public WeatherController(OpenWeatherApiService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("search/{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            var weather = await _weatherService.GetWeatherAsync(city);

            if (weather != null)
            {
                return Ok(weather);
            }

            return NotFound();
        }
    }

}
