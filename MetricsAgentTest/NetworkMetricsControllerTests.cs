using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTest
{

    public class NetworkMetricsControllerTests
    {
        private NetworkMetricsController _networkMetricsController;

        public NetworkMetricsControllerTests()
        {
            _networkMetricsController = new NetworkMetricsController();
        }

        [Fact]
        public void GetRemainingSpace_ReturnOk()
        {
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);
            var result = _networkMetricsController.GetNetworkLoad(fromTime, toTime);
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
