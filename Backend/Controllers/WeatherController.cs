using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace WhoKnows_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // Inject HttpClient for making API calls
        public WeatherController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // Endpoint to fetch weather based on location
        [HttpGet("")]
        public async Task<IActionResult> GetWeather()
        {
            try
            {
                // Make sure to use an absolute URL for the external API
                var apiKey = _configuration.GetValue<String>("apiKey:apiKey"); // Move this to appsettings.json for security

                var url = $"https://api.openweathermap.org/data/2.5/forecast?lat=55.69166038504486&lon=12.554674149615163&exclude=hourly,daily&appid={apiKey}&units=metric";

                var response = await _httpClient.GetAsync(url);

                // Ensure successful response
                if (response.IsSuccessStatusCode)
                {
                    var weatherData = await response.Content.ReadAsStringAsync();
                    return Ok(weatherData); // Return the raw response from the weather API
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to retrieve weather data.");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }
        }
    }
}