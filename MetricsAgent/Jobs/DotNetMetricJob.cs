using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    //gc-heap-size
    public class DotNetMetricJob : IJob
    {
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;
        //private PerformanceCounter _dotNetCounter;

        public DotNetMetricJob(IDotNetMetricsRepository dotNetMetricsRepository)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;
            //_dotNetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", "_Global_");

        }

        public Task Execute(IJobExecutionContext context)
        {
            //Debug.WriteLine($"{DateTime.Now} > DotNetMetricJob");

            // Получаем значение занятости CPU
            //float dotNetUsageInPercents = _dotNetCounter.NextValue();
            long dotNetUsageInPercents = GC.GetTotalAllocatedBytes();  //float dotNetUsageInPercents = _dotNetCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _dotNetMetricsRepository.Create(new DotNetMetric
            {
                Time = time.TotalSeconds,
                Value = (int)dotNetUsageInPercents / 1024 //перевожу в МБайты
            });
            return Task.CompletedTask;
        }
    }
}
