using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WiserService.Messages;
using Timer = System.Timers.Timer;

namespace WiserService
{
    public class DataLoggerTimerWorker : IHostedService, IDisposable
    {
        private readonly IMediator mediator;
        private readonly WiserServiceOptions options;
        private readonly ILogger<DataLoggerTimerWorker> logger;
        private Timer timer;

        public DataLoggerTimerWorker(
            IMediator mediator,
            IOptions<WiserServiceOptions> options,
            ILogger<DataLoggerTimerWorker> logger)
        {
            this.mediator = mediator;
            this.options = options.Value;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            this.logger.LogTrace($"{nameof(DataLoggerTimerWorker)} is starting...");

            this.timer = new Timer(options.PollingInterval.TotalMilliseconds)
            {
                AutoReset = true,
            };

            this.timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            this.timer.Start();

            this.logger.LogInformation($"{nameof(DataLoggerTimerWorker)} is started.");

            return Task.CompletedTask;
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            this.logger.LogInformation($"{nameof(DataLoggerTimerWorker)} triggered.");
            await this.mediator.Publish(new LogWiserDataCommand());
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this.logger.LogTrace($"{nameof(DataLoggerTimerWorker)} is stopping...");

            this.timer.Stop();

            this.logger.LogInformation($"{nameof(DataLoggerTimerWorker)} is stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }
    }
}
