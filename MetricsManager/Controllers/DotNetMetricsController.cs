using MetricsManager.Models.Requests;
using MetricsManager.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/dotnet")]
    [ApiController]
    [SwaggerTag("Предоставляет работу с метрикой размеров выделенной памяти для переменных (Garbage Collection)")]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly IMetricsAgentClient _metricsAgentClient;

        public DotNetMetricsController(
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
            DotNetMetricsResponse response = _metricsAgentClient.GetDotNetMetrics(new DotNetMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(response);
        }
    }
}

