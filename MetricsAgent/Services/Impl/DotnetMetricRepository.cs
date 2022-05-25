using MetricsAgent.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Impl
{
    public class DotnetMetricRepository : IDotnetMetricsRepository
    {
        public void Create(DotnetMetric item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<DotnetMetric> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public DotnetMetric GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<DotnetMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            throw new NotImplementedException();
        }

        public void Update(DotnetMetric item)
        {
            throw new System.NotImplementedException();
        }
    }
}
