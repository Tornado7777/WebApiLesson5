using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTest
{

    public class HddMetricsControllerTests
    {
        private HddMetricsController _hddMetricsController;

        public HddMetricsControllerTests()
        {
            _hddMetricsController = new HddMetricsController();
        }

        [Fact]
        public void GetRemainingSpace_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _hddMetricsController.GetRemainingSpace(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
