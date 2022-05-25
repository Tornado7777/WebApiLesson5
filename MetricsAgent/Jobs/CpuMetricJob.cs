using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {

        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private PerformanceCounter _cpuCounter;

        public CpuMetricJob(ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //Debug.WriteLine($"{DateTime.Now} > CpuMetricJob");

            // Получаем значение занятости CPU
            float cpuUsageInPercents = _cpuCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _cpuMetricsRepository.Create(new CpuMetric
            {
                Time = time.TotalSeconds,
                Value = (int)cpuUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
