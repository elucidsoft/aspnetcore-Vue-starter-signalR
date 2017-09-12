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
        private readonly Counter _counter;

        public CounterHub(Counter counter)
        {
            _counter = counter;
        }
    }

    public class Counter
    {
        private static int _counter = 0;
 
        public Counter(IHubContext<CounterHub> context)
        {
            Clients = context.Clients;
            StartCounter();
        }

        private IHubClients Clients { get; }

        public void StartCounter()
        {
            var cancellationToken = new CancellationToken();
            Task.Run(async () =>
            {
                while (true)
                {
                    Increment();
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

        private async void Increment()
        {
            var mutex = new SemaphoreSlim(1);
            await mutex.WaitAsync();
            try
            {
                _counter++;
                 await Clients.All.InvokeAsync("increment", _counter);
            }
            finally
            {
                mutex.Release();
            }
        }
    }
}
