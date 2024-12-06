using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qui_test_api.Database;
using qui_test_api.Models;
using qui_test_api.WeatherApiIntegration;

namespace qui_test_api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherApiInterface _weatherService;
        private readonly HistoryContext _databaseContext;

        public WeatherController(WeatherApiInterface weatherService, HistoryContext databaseContext)
        {
            _weatherService = weatherService;
            _databaseContext = databaseContext;
        }

        [HttpGet("search/{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            var userIdentifier = Request.Headers["User-Identifier"].ToString();

            if (string.IsNullOrWhiteSpace(userIdentifier))
            {
                return BadRequest(new ErrorResponse
                {
                    Status = 400,
                    Message = "Bad Request",
                    Details = "User identifier is missing"
                });
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);

                if (weather != null)
                {
                    var userRequestData = new HistoryRecord
                    {
                        Id = Guid.NewGuid(),
                        CityName = city,
                        QueryDate = DateTime.UtcNow,
                        UserIdentifier = userIdentifier,
                        WeatherData = weather
                    };

                    _databaseContext.Add(userRequestData);
                    await _databaseContext.SaveChangesAsync();

                    return Ok(weather);
                }

                return NotFound(new ErrorResponse
                {
                    Status = 404,
                    Message = "Not Found",
                    Details = $"Weather data for city {city} not found."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetUserHistory()
        {
            var userIdentifier = Request.Headers["User-Identifier"].ToString();
            if (string.IsNullOrWhiteSpace(userIdentifier))
            {
                return BadRequest("User identifier is missing");
            }

            var userHistory = await _databaseContext.HistoryRecords.Where(r => r.UserIdentifier == userIdentifier).ToListAsync();

            return Ok(userHistory);
        }
    }

}
