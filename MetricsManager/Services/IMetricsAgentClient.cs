using MetricsManager.Models.Requests;

namespace MetricsManager.Services
{
    public interface IMetricsAgentClient
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest cpuMetricsRequest);

        //TODO: Домашнаяя работа 6
        // Добавить методы для получения метрик
    }
}
