using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Vue2SpaSignalR.DataProviders;

namespace Vue2SpaSignalR.Services.Hubs
{
    public class WeatherHub : Hub
    {
        private static Timer _timer;

        public WeatherHub()
        {
            var weatherDataProvider = new WeatherDataProvider();

            _timer = new Timer(state =>
            {
                Clients.All.InvokeAsync("weather", weatherDataProvider.WeatherForecasts());

            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5));
        }
    }
}
