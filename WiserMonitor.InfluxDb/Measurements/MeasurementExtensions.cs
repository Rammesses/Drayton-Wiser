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
                Name = roomData.Name,
                Temperature = roomData.CalculatedTemperature,
                SetPoint = roomData.CurrentSetPoint,
                Demand = roomData.PercentageDemand,
                TimeStamp = roomData.DataDate
            };

            return measurement;
        }
    }
}
