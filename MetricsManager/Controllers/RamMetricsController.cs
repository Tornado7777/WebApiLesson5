using MetricsManager.Models.Requests;
using MetricsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/ram")]
    [ApiController]
    [SwaggerTag("Предоставляет работу с метрикой загрузки памяти")]
    public class RamMetricsController : ControllerBase
    {
        private readonly IMetricsAgentClient _metricsAgentClient;

        public RamMetricsController(
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
            RamMetricsResponse response = _metricsAgentClient.GetRamMetrics(new RamMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(response);
        }
    }
}
