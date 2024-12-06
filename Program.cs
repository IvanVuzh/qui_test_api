using Microsoft.EntityFrameworkCore;

using qui_test_api.Controllers;
using qui_test_api.Database;
using qui_test_api.WeatherApiIntegration;
using qui_test_api.WeatherApiIntegration.OpenWeatherApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<HistoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.WebHost.UseUrls("https://localhost:5001", "http://localhost:5000");

// Add services to the container.
builder.Services.AddControllers(); 

builder.Services.Configure<OpenWeatherSettings>(builder.Configuration.GetSection("OpenWeatherSettings")); 

builder.Services.AddHttpClient(); 

builder.Services.AddScoped<WeatherApiInterface, OpenWeatherApiService>();

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

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
