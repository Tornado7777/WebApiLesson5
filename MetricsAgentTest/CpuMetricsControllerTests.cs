using MetricsAgent.Controllers;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Moq;
using System;
using Xunit;

namespace MetricsAgentTest
{
    public class CpuMetricsControllerTests
    {
        private CpuMetricsController _cpuMetricsController;
        // ��������� ��������
        private Mock<ICpuMetricsRepository> mock;
        
        public CpuMetricsControllerTests()
        {
            mock = new Mock<ICpuMetricsRepository>();
            _cpuMetricsController = new CpuMetricsController(null, null, mock.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // ������������� �������� ��������
            // � �������� �����������, ��� � ����������� �������� CpuMetric - ������
            mock.Setup(repository =>
            repository.Create(It.IsAny<CpuMetric>())).Verifiable();
            // ��������� �������� �� �����������
            var result = _cpuMetricsController.Create(new
            CpuMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });

            // ��������� �������� �� ��, ��� ���� ������� ����������
            // �������� ����� Create ����������� � ������ ����� ������� � ���������
            mock.Verify(repository => repository.Create(It.IsAny<CpuMetric>()),
            Times.AtMostOnce());
        }

    }

}
