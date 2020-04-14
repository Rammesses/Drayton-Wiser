using System;
using InfluxDB.Client.Core;

namespace WiserMonitor.InfluxDb.Measurements
{
    [Measurement("room_temp")]
    public class RoomDataMeasurement
    {
        public Guid Id { get; internal set; }

        [Column("host", IsTag = true)]
        public string Host { get; internal set; }

        [Column("room", IsTag = true)]
        public string Name { get; internal set; }

        [Column("temp")]
        public int Temperature { get; internal set; }

        [Column("set_temp")]
        public int SetPoint { get; internal set; }

        [Column("percentage_demand")]
        public int Demand { get; internal set; }

        [Column(IsTimestamp = true)]
        public DateTime TimeStamp { get; internal set; }
    }
}