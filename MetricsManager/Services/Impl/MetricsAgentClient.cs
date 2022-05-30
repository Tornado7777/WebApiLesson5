using MetricsManager.Models;
using MetricsManager.Models.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;

namespace MetricsManager.Services.Impl
{
    public class MetricsAgentClient : IMetricsAgentClient
    {

        #region Services

        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsAgentClient> _logger;
        private readonly AgentPool _agentPoll;

        #endregion

        public MetricsAgentClient(
            ILogger<MetricsAgentClient> logger,
            HttpClient httpClient,
            AgentPool agentPoll)
        {
            _httpClient = httpClient;
            _logger = logger;
            _agentPoll = agentPoll;
        }

        public CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest cpuMetricsRequest)
        {
            try
            {
                AgentInfo agentInfo = _agentPoll.Get().FirstOrDefault(agent => agent.AgentId == cpuMetricsRequest.AgentId);
                if (agentInfo == null)
                    throw new Exception($"AgentId #{cpuMetricsRequest.AgentId} not found.");

                //AgentInfo agentInfo = new AgentInfo();
                //agentInfo.AgentAddress = new Uri("https://localhost:44339/");
                //agentInfo.AgentId = 1;
                //agentInfo.Enable = true;


                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/cpu/from/{cpuMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{cpuMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                //HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(CpuMetricsResponse));
                    cpuMetricsResponse.AgentId = cpuMetricsRequest.AgentId;
                    return cpuMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return null;
        }
    }
}
