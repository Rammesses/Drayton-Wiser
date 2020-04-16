using System;
using InfluxDB.Client.Core;

namespace WiserMonitor.InfluxDb.Measurements
{
    [Measurement("temp")]
    public class RoomDataMeasurement
    {
        [Column("host", IsTag = true)]
        public string Host { get; internal set; }

        [Column("room", IsTag = true)]
        public string Name { get; internal set; }

        [Column("calc_temp")]
        public decimal Temperature { get; internal set; }

        [Column("set_temp")]
        public decimal SetPoint { get; internal set; }

        [Column("demand")]
        public int Demand { get; internal set; }

        [Column(IsTimestamp = true)]
        public DateTime TimeStamp { get; internal set; }
    }
}