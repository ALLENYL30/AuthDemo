using AuthDemo.Api.Conts;
using AuthDemo.Api.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,IMySession session)
    {
        _logger = logger;
        Session = session;
    }

    public IMySession Session { get; }

    [HttpGet(Name = "GetWeatherForecast")]
    [Authorize()]
    public IEnumerable<WeatherForecast> Get()
    {
        Session.SetString("UserName","Ace");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [HttpGet("test")]
    [Authorize()]
    public string GetTest()
    {
        return Session.GetString("UserName");
    }
}
