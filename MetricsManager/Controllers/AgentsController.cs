using MetricsManager.Models;
using Microsoft.AspNetCore.Mvc;


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
            _agentPool.EnableAgentById(agentId);
            return Ok();
        }
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _agentPool.DisableAgentById(agentId);
            return Ok();
        }

        [HttpGet("get")]
        public IActionResult GetAllAgents()
        {
            return Ok(_agentPool.Get());
        }

    }
}
