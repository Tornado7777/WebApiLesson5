using MetricsAgent.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Impl
{
    public class RamMetricRepository : IRamMetricsRepository
    {
        public void Create(RamMetric item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<RamMetric> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public RamMetric GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<RamMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            throw new NotImplementedException();
        }

        public void Update(RamMetric item)
        {
            throw new System.NotImplementedException();
        }
    }
}
