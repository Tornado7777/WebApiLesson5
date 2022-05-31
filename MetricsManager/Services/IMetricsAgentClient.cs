using MetricsManager.Models.Requests;

namespace MetricsManager.Services
{
    public interface IMetricsAgentClient
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest cpuMetricsRequest);
        DotNetMetricsResponse GetDotNetMetrics(DotNetMetricsRequest dotNetMetricsRequest);
        HddMetricsResponse GetHddMetrics(HddMetricsRequest hddMetricsRequest);
        NetworkMetricsResponse GetNetworkMetrics(NetworkMetricsRequest networkMetricsRequest);
        RamMetricsResponse GetRamMetrics(RamMetricsRequest ramMetricsRequest);

        //TODO: Домашнаяя работа 6
        // Добавить методы для получения метрик
    }
}
