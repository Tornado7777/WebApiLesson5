using System;

namespace MetricsManager.Models
{
    public class RamMetric
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }

        public int Value { get; set; }

    }
}
