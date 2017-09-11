using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Vue2SpaSignalR.Services.Hubs
{
    public class CounterHub : Hub
    {
        private static Timer _timer;
        private static int _counter = 0;

        public CounterHub()
        {
            _timer = new Timer(state =>
            {
                var val = _counter++;
                Clients.All.InvokeAsync("counter", _counter);

            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }
    }

}
