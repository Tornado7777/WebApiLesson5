using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //Debug.WriteLine($"{DateTime.Now} > RamMetricJob");

            // Получаем значение занятости Ram
            float ramUsageInPercents = _ramCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _ramMetricsRepository.Create(new RamMetric
            {
                Time = time.TotalSeconds,
                Value = (int)ramUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
