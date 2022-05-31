using MetricsManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;
using MetricsManager.Models.Requests;
using MetricsManager.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsManager.Controllers
{
    /// <summary>
    /// Получение данных CpuMetrics
    /// </summary>
    [Route("api/cpu")]
    [ApiController]
    [SwaggerTag("Предоставляет работу с метрикой загрузки процессора")]
    public class CpuMetricsController : ControllerBase
    {

        private readonly IMetricsAgentClient _metricsAgentClient;

        public CpuMetricsController(
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
            CpuMetricsResponse response = _metricsAgentClient.GetCpuMetrics(new CpuMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });
            return Ok(response);
        }
    }
}
