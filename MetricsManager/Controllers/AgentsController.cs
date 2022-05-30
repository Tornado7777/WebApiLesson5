using Dapper;
using MetricsManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {

        private readonly IOptions<DatabaseOptions> _databaseOptions;
        //private AgentPool _agentPool;

        public AgentsController(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            string enableStr;
            if (agentInfo.Enable) enableStr = "true"; else enableStr = "false";

            // Запрос на добавление данных с плейсхолдерами для параметров
            string querySQL = string.Format("INSERT INTO metricsagents(agentaddress, enable) VALUES('{0}', {1})", agentInfo.AgentAddress, enableStr);
            connection.Execute(querySQL);
            //connection.Execute("INSERT INTO metricsagents(agentaddress, enable) VALUES(@agentAdress, @enable)",
            //// Анонимный объект с параметрами запроса
            //new
            //{
               
            //    agentaddress = agentInfo.AgentAddress,
            //    enable = enableStr
            //});

           
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("UPDATE metricsagents SET enable = 1 WHERE AgentId = @id",
            new
            {
               id = agentId
            });

            return Ok();
        }
        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("UPDATE metricsagents SET enable = 0 WHERE AgentId = @id",
            new
            {
                id = agentId
            });

            return Ok();
        }

        [HttpGet("get")]
        public List<AgentInfo> GetAllAgents()
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            //Id, Time, Value
            List<AgentInfo> metricsAgents = connection.Query<AgentInfo>("SELECT * FROM metricsagents").ToList();
            return metricsAgents;
        }

    }
}
