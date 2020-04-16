using System;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using Wiser.DataLogger;
using Wiser.DataObjects;

namespace WiserMonitor.InfluxDb.Measurements
{
    public static class MeasurementExtensions
    {
        public static RoomDataMeasurement AsRoomDataMeasurement(this Room room)
        {
            var calcTemp = Convert.ToDecimal(room.CalculatedTemperature / 10.0);
            var setTemp = Convert.ToDecimal(room.CurrentSetPoint / 10.0);
            var measurement = new RoomDataMeasurement()
            {
                Host = Environment.MachineName,
                Name = room.Name,
                Temperature = calcTemp,
                SetPoint = setTemp,
                Demand = room.PercentageDemand,
                TimeStamp = DateTime.UtcNow
            };

            return measurement;
        }

        public static PointData AsPointData(this Room room)
        {
            var calcTemp = (room.CalculatedTemperature / 10.0);
            var setTemp = (room.CurrentSetPoint / 10.0);
            var measurement = PointData.Measurement("temp")
                .Tag("room", room.Name)
                .Tag("host", Environment.MachineName.ToLowerInvariant())
                .Field("calc_temp", calcTemp)
                .Field("set_temp", setTemp)
                .Field("demand", room.PercentageDemand)
                .Timestamp(DateTime.UtcNow, WritePrecision.S);
            return measurement;
        }
    }
}
