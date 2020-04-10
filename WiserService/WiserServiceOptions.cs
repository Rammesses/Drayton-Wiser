using System;
using System.Threading;

namespace WiserService
{
    public class WiserServiceOptions
    {
        public TimeSpan PollingInterval { get; internal set; } = TimeSpan.FromSeconds(15);
    }
}