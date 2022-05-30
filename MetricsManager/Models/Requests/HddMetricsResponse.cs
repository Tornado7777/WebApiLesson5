namespace MetricsManager.Models.Requests
{
    public class HddMetricsResponse
    {
        public int AgentId { get; set; }
        public HddMetric[] Metrics { get; set; }
    }
}
