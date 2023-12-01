namespace webapi.Endpoints
{
    public class WeatherEndpoint
    {
        private string[] summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }

        public WeatherEndpoint(WebApplication app)
        {
            app.MapGet("/weatherforecast", GetWeatherForecast)
                .WithName("GetWeatherForecast")
                .WithOpenApi();
        }

        private IEnumerable<WeatherForecast> GetWeatherForecast(HttpContext context)
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    }
}
