﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qui_test_api.WeatherApiIntegration;
using qui_test_api.WeatherApiIntegration.OpenWeatherApi;

public class OpenWeatherApiService: WeatherApiInterface
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly OpenWeatherSettings _settings;
    private readonly ILogger<OpenWeatherApiService> _logger;

    public OpenWeatherApiService(HttpClient httpClient, IMemoryCache cache, IOptions<OpenWeatherSettings> options, ILogger<OpenWeatherApiService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _settings = options.Value;
        _logger = logger;
    }


    public async Task<string> GetWeatherAsync(string city)
    {
        string cacheKey = $"Weather_for_{city}";

        if (_cache.TryGetValue(cacheKey, out string weather))
        {
            return weather;
        }

        var url = $"{_settings.BaseUrl}weather?q={city}&units=metric&appid={_settings.ApiKey}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _cache.Set(cacheKey, content, TimeSpan.FromMinutes(30));
                return content;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;

                _logger.LogError("Failed to get weather data. Status code: {StatusCode}, Response: {ErrorContent}", statusCode, errorContent);

                throw new HttpRequestException($"Request failed with status code {statusCode} and response: {errorContent}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"HTTP request error while getting weather data for city {city}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error while getting weather data for city {city}");
            throw;
        }
    }

}
