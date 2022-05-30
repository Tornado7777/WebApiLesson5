namespace MetricsManager.Models.Requests
{
    public class CpuMetricsResponse
    {
        public int AgentId { get; set; }
        public CpuMetric[] Metrics { get; set; }
    }
}
