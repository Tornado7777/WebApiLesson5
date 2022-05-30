namespace MetricsManager.Models.Requests
{
    public class RamMetricsResponse
    {
        public int AgentId { get; set; }
        public RamMetric[] Metrics { get; set; }
    }
}
