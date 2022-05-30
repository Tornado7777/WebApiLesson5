using MetricsManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;
using MetricsManager.Models.Requests;
using MetricsManager.Services;

namespace MetricsManager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AgentPool _agentPoll;
        private readonly IMetricsAgentClient _metricsAgentClient;

        public CpuMetricsController(
            IMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory,
            AgentPool agentPoll)
        {
            _agentPoll = agentPoll;
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
        }

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

        [HttpGet("agentOld1/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgentOld1(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            AgentInfo agentInfo = _agentPoll.Get().FirstOrDefault(agent => agent.AgentId == agentId);
            if (agentInfo == null)
                return BadRequest();

            //AgentInfo agentInfo = new AgentInfo();
            //agentInfo.AgentAddress = new Uri("https://localhost:44339/");
            //agentInfo.AgentId = 1;
            //agentInfo.Enable = true;


            string requestQuery =
                $"{agentInfo.AgentAddress}api/metrics/cpu/from/{fromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{toTime.ToString("dd\\.hh\\:mm\\:ss")}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = httpClient.SendAsync(httpRequestMessage).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(CpuMetricsResponse));
                cpuMetricsResponse.AgentId = agentId;
                return Ok(cpuMetricsResponse);
            }
            return BadRequest();
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
