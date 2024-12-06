using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace qui_test_api.WeatherApiIntegration.OpenWeatherApi
{
    public class OpenWeatherApiService: WeatherApiInterface
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherSettings _settings;
        private readonly IMemoryCache _cache;

        public OpenWeatherApiService(HttpClient httpClient, IOptions<OpenWeatherSettings> settings, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _cache = cache;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            string cacheKey = $"Weather_for_{city}";
            if (_cache.TryGetValue(cacheKey, out string weather))
            {
                return weather;
            }

            var url = $"{_settings.BaseUrl}weather?q={city}&APPID={_settings.ApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                _cache.Set(cacheKey, content, TimeSpan.FromMinutes(30));

                return content;
            }

            return null;
        }
    }

}
