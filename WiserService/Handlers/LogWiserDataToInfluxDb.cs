using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using WiserService.Messages;

namespace WiserService.Handlers
{
    public class LogWiserDataToInfluxDb : INotificationHandler<LogWiserDataCommand>
    {
        private readonly ILogger logger;

        public LogWiserDataToInfluxDb(
            ILogger<LogWiserDataToInfluxDb> logger)
        {
            this.logger = logger;
        }

        Task INotificationHandler<LogWiserDataCommand>.Handle(LogWiserDataCommand notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Logged hub data to InfluxDb");

            return Task.CompletedTask;
        }
    }
}
