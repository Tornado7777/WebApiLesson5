using MetricsManager.Models.Requests;
using MetricsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;


namespace MetricsManager.Controllers
{
    [Route("api/network")]
    [ApiController]
    [SwaggerTag("Предоставляет работу с метрикой коливо полученндых данных за секунду")]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly IMetricsAgentClient _metricsAgentClient;

        public NetworkMetricsController(
            IMetricsAgentClient metricsAgentClient)
        {
            _metricsAgentClient = metricsAgentClient;
        }


        /// <summary>
        /// Получение данных в диапазоне времени
        /// </summary>
        /// <param name="agentId"> Id  метрикс агента в БД</param>
        /// <param name="fromTime">с</param>
        /// <param name="toTime">по</param>
        /// <returns></returns>
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            NetworkMetricsResponse response = _metricsAgentClient.GetNetworkMetrics(new NetworkMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(response);
        }
    }
}
