using qui_test_api.Controllers;
using qui_test_api.WeatherApiIntegration.OpenWeatherApi;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:5001", "http://localhost:5000");

// Add services to the container.
builder.Services.AddControllers(); 
builder.Services.Configure<OpenWeatherSettings>(builder.Configuration.GetSection("OpenWeatherSettings")); 
builder.Services.AddHttpClient(); 
builder.Services.AddTransient<OpenWeatherApiService>(); 
builder.Services.AddMemoryCache(); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<WeatherController>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
