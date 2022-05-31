using System;

namespace MetricsManager.Models.Requests
{
    public class NetworkMetricsRequest
    {
        public int AgentId { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
