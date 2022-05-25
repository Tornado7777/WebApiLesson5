using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTest
{
    public class RamMerticsControllerTests
    {
        private RamMerticsController _ramMetricsController;

        public RamMerticsControllerTests()
        {
            _ramMetricsController = new RamMerticsController();
        }

        [Fact]
        public void GetRamAvailable_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _ramMetricsController.GetRamAvailable(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
