using dotnet_learning.Data;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_learning.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    
    public WeatherForecastController(DatabaseContext dbContext)
    {
        DbContext = dbContext;
    }
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public DatabaseContext DbContext { get; set;}

    [HttpGet("test")]
    public IActionResult GetTModel()
    {
        var result = DbContext.Products.ToList();
        return Ok(result);
    }
    

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
