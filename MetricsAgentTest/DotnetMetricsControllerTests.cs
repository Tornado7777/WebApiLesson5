using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTest
{

    public class DotnetMetricsControllerTests
    {
        private DotnetMetricsController _dotnetMetricsController;

        public DotnetMetricsControllerTests()
        {
            _dotnetMetricsController = new DotnetMetricsController();
        }

        [Fact]
        public void GetErrorsCount_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _dotnetMetricsController.GetErrorsCount(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }

    }
}
