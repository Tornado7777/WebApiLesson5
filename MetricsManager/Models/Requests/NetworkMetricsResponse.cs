namespace MetricsManager.Models.Requests
{
    public class NetworkMetricsResponse
    {
        public int AgentId { get; set; }
        public NetworkMetric[] Metrics { get; set; }
    }
}
