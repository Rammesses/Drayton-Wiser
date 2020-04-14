using System.Collections.Generic;
using System;
using Wiser.DataLogger;
using Wiser.DataObjects;
using Microsoft.Extensions.Options;
using InfluxDB.Client;
using WiserMonitor.InfluxDb.Measurements;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace WiserMonitor.InfluxDb
{
    public class InfluxDbDataLogger : IDataLogger
    {
        private readonly InfluxDbDataLoggerOptions options;
        private readonly InfluxDBClient client;

        public InfluxDbDataLogger(IOptions<InfluxDbDataLoggerOptions> options)
        {
            this.options = options.Value;            

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
                    var newData = PointData.Measurement("temp")
                        .Tag("room", room.Name)
                        .Tag("host", Environment.MachineName.ToLowerInvariant())
                        .Field("calc_temp", room.CalculatedTemperature)
                        .Field("set_temp", room.CurrentSetPoint)
                        .Field("demand", room.PercentageDemand)
                        .Timestamp(DateTime.UtcNow.Ticks, WritePrecision.S);

                    writeApi.WritePoint(this.options.BucketId, this.options.OrgId, newData);
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
