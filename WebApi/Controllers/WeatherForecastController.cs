using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sentry;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        
        [HttpGet("exception")]
        public bool Exception()
        {
            try
            {
                throw new Exception("Intentional exception");
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return false;
        }

        [HttpGet("division")]
        public int Division([FromQuery] int a, [FromQuery] int b)
        {
            return a / b;
        }


        [HttpGet("null")]
        public bool Null()
        {
            var a = GetFirstChar(null);
            return a == 'a';
        }

        private char GetFirstChar(string str)
        {
            return str[0];
        }
    }
}
