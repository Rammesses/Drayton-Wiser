using Wiser.DataLogger;

namespace WiserMonitor.InfluxDb
{
    public class InfluxDbDataLoggerOptions : IDataLoggerOptions<InfluxDbDataLogger>
    {
        public string ConnectionString { get; set; }
        public string Token { get; set; }

        public string BucketId { get; set; }

        public string OrgId { get; set; }
    }
}