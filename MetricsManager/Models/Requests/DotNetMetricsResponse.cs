namespace MetricsManager.Models.Requests
{
    public class DotNetMetricsResponse
    {
        public int AgentId { get; set; }
        public DotNetMetric[] Metrics { get; set; }
    }
}
