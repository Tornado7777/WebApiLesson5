using MetricsAgent.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Impl
{
    public class NetworkMetricRepository : INetworkMetricsRepository
    {
        public void Create(NetworkMetric item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<NetworkMetric> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public NetworkMetric GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<NetworkMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            throw new NotImplementedException();
        }

        public void Update(NetworkMetric item)
        {
            throw new System.NotImplementedException();
        }
    }
}
