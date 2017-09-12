using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace Vue2SpaSignalR.Services.Hubs
{
    public class WeatherHub : Hub
    {
        private readonly Weather _weather;

        public WeatherHub(Weather weather)
        {
            _weather = weather;
            weather.StartWeatherFetch();
        }
    }

    public class Weather
    {
        private ConcurrentBag<WeatherForecast> weatherForecasts = new ConcurrentBag<WeatherForecast>();

        public Weather(IHubContext<WeatherHub> context)
        {
            Clients = context.Clients;
        }

        public void StartWeatherFetch()
        {
            var cancellationToken = new CancellationToken();
            Task.Run(async () =>
            {
                while (true)
                {
                    GetWeatherForecasts();
                    var task = Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    try
                    {
                        await task;
                    }
                    catch (TaskCanceledException)
                    {
                        return;
                    }
                }
            }, cancellationToken);
        }

        private IHubClients Clients { get; }

        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async void GetWeatherForecasts()
        {
            var rng = new Random();
            var randomWeatherForescast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            weatherForecasts.Clear();
            foreach (var wf in randomWeatherForescast)
            {
                weatherForecasts.Add(wf);
            }

            await Clients.All.InvokeAsync("weather", weatherForecasts.AsEnumerable());
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }
    }
}
