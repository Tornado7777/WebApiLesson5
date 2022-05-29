using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private PerformanceCounter _hddCounter;

        public HddMetricJob(IHddMetricsRepository hddMetricsRepository)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            //Debug.WriteLine($"{DateTime.Now} > HddMetricJob");

            // Получаем значение занятости CPU
            float hddUsageInPercents = _hddCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _hddMetricsRepository.Create(new HddMetric
            {
                Time = time.TotalSeconds,
                Value = (int)hddUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
