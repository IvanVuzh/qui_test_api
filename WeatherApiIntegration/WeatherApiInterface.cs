namespace qui_test_api.WeatherApiIntegration
{
    public interface WeatherApiInterface
    {
        public Task<string> GetWeatherAsync(string city);
    }
}
