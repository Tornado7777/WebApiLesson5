using MetricsAgent.Models;
using MetricsAgent.Services;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private PerformanceCounterCategory _categoryNetwork;

        public NetworkMetricJob(INetworkMetricsRepository networkMetricsRepository)
        {
            _networkMetricsRepository = networkMetricsRepository;
            _categoryNetwork = new PerformanceCounterCategory("Network Interface");
        }

        public Task Execute(IJobExecutionContext context)
        {
            String[] nameNetwork = _categoryNetwork.GetInstanceNames();
            float networkUsageRecivedInSec = 0;

            foreach (string name in nameNetwork)
            {
                var _networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
                //суммирую полученный трафик
                networkUsageRecivedInSec += _networkCounter.NextValue();

            }

            //Усредняю
            //networkUsageRecivedInSec /= nameNetwork.Length;

            
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _networkMetricsRepository.Create(new NetworkMetric
            {
                Time = time.TotalSeconds,
                Value = (int)networkUsageRecivedInSec
            });
            return Task.CompletedTask;
        }
    }
}
