using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DowntimeAlerter.Infrastructure.Services
{
    public class CheckApplicationTask : IHostedService, IDisposable
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CheckApplicationTask> _logger;
        private Timer _timer;

        public CheckApplicationTask(ILogger<CheckApplicationTask> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            _logger.LogInformation("Background Service started.");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "https://www.google.com/");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Application is still working.");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Application has been stopped.");
                Console.WriteLine(e);
            }

            _logger.LogInformation("Background Service finished.");
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}