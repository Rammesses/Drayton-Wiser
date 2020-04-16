using System.Collections.Generic;
using System;
using Wiser.DataLogger;
using Wiser.DataObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InfluxDB.Client;
using WiserMonitor.InfluxDb.Measurements;
using InfluxDB.Client.Api.Domain;

namespace WiserMonitor.InfluxDb
{
    public class InfluxDbDataLogger : IDataLogger
    {
        private readonly InfluxDbDataLoggerOptions options;
        private readonly ILogger logger;
        private readonly InfluxDBClient client;

        public InfluxDbDataLogger(
            IOptions<InfluxDbDataLoggerOptions> options,
            ILogger<InfluxDbDataLogger> logger)
        {
            this.options = options.Value;
            this.logger = logger;
            this.client = InfluxDBClientFactory.Create(this.options.ConnectionString, this.options.Token.ToCharArray());
        }

        public List<RoomData> GetRoomData(string roomName, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void WriteRoomData(HeatHub hub)
        {
            using (var writeApi = client.GetWriteApi())
            {
                foreach (var room in hub.Room)
                {
                    if (room.CalculatedTemperature < -1000.0)
                    {
                        this.logger.LogWarning("{roomName} is not reporting temperature ({reportedTemp}/{alexaTemp}).", room.Name, room.CalculatedTemperature, room.RoundedAlexaTemperature);
                        continue;
                    }

                    writeApi.WriteMeasurement(this.options.BucketId, this.options.OrgId, WritePrecision.S, room.AsRoomDataMeasurement());
                }

                writeApi.Flush();
            }
        }

        public void WriteValveData(HeatHub hub)
        {
            throw new NotImplementedException();
        }
    }
}
