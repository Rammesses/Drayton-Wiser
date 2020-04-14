using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Wiser;
using Wiser.DataLogger;
using WiserService.Messages;

namespace WiserService.Handlers
{
    public class LogWiserDataToDataLoggers : INotificationHandler<LogWiserDataCommand>
    {
        private readonly IWiserDataProvider dataProvider;
        private readonly IList<IDataLogger> dataLoggers;
        private readonly ILogger logger;

        public LogWiserDataToDataLoggers(
            IWiserDataProvider dataProvider,
            IEnumerable<IDataLogger> dataLoggers,
            ILogger<LogWiserDataToDataLoggers> logger)
        {
            this.dataProvider = dataProvider;
            this.dataLoggers = dataLoggers.ToList();
            this.logger = logger;
        }

        Task INotificationHandler<LogWiserDataCommand>.Handle(LogWiserDataCommand notification, CancellationToken cancellationToken)
        {
            var data = this.dataProvider.GetData();
            foreach (var dataLogger in this.dataLoggers)
            {
                dataLogger.WriteRoomData(data);
            }

            this.logger.LogInformation("Logged hub data to {loggerCount} data loggers", this.dataLoggers.Count);

            return Task.CompletedTask;
        }
    }
}
