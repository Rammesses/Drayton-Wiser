using System.Collections.Generic;
using System;
using Wiser.DataLogger;
using Wiser.DataObjects;
using Microsoft.Extensions.Options;
using InfluxDB.Client;
using WiserMonitor.InfluxDb.Measurements;
using InfluxDB.Client.Api.Domain;

namespace WiserMonitor.InfluxDb
{
    public class InfluxDbDataLogger : IDataLogger
    {
        private readonly InfluxDbDataLoggerOptions options;
        private readonly InfluxDBClient client;

        public InfluxDbDataLogger(IOptions<InfluxDbDataLoggerOptions> options)
        {
            this.options = options.Value;
            this.client = InfluxDBClientFactory.Create(this.options.ConnectionString);
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
                    var newData = new RoomData
                    {
                        id = room.id,
                        DataDate = DateTime.Now,
                        Name = room.Name,
                        CalculatedTemperature = room.CalculatedTemperature,
                        CurrentSetPoint = room.CurrentSetPoint,
                        PercentageDemand = room.PercentageDemand
                    };

                    writeApi.WriteMeasurement(WritePrecision.S, newData.AsMeasurement());
                }
            }
        }

        public void WriteValveData(HeatHub hub)
        {
            throw new NotImplementedException();
        }
    }
}
