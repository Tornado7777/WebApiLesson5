using MetricsAgent.Models;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Services.Impl
{
    public class HddMetricRepository : IHddMetricsRepository
    {
        public void Create(HddMetric item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<HddMetric> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public HddMetric GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IList<HddMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            throw new NotImplementedException();
        }

        public void Update(HddMetric item)
        {
            throw new System.NotImplementedException();
        }
    }
}
