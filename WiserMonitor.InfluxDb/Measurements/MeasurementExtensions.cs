using System;
using Wiser.DataLogger;

namespace WiserMonitor.InfluxDb.Measurements
{
    public static class MeasurementExtensions
    {
        public static RoomDataMeasurement AsMeasurement(this RoomData roomData)
        {
            var measurement = new RoomDataMeasurement()
            {
                Id = roomData.Id,
                Host = Environment.MachineName,
                Name = roomData.Name,
                Temperature = roomData.CalculatedTemperature,
                SetPoint = roomData.CurrentSetPoint,
                Demand = roomData.PercentageDemand,
                TimeStamp = new DateTime(roomData.DataDate.Ticks, DateTimeKind.Utc)
            };

            return measurement;
        }
    }
}
