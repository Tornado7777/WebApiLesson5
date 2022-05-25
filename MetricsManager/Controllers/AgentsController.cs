using MetricsManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {

        private AgentPool _agentPool;

        public AgentsController(AgentPool agentPool)
        {
            _agentPool = agentPool;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            if (agentInfo != null)
            {
                _agentPool.Add(agentInfo);
            }
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Values.ContainsKey(agentId))
                _agentPool.Values[agentId].Enable = true;
            return Ok();
        }
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Values.ContainsKey(agentId))
                _agentPool.Values[agentId].Enable = false;
            return Ok();
        }

        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            return Ok(_agentPool.Get());
        }

    }
}
