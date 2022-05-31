using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace MetricsManager.Models
{
    public class AgentPool
    {
        private Dictionary<int, AgentInfo> _values;
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public AgentPool(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            List<AgentInfo> metricsAgents = connection.Query<AgentInfo>("SELECT * FROM metricsagents").ToList();
            _values = new Dictionary<int, AgentInfo>();
            foreach (AgentInfo agentInfo in metricsAgents)
            {
                _values.Add(agentInfo.AgentId, agentInfo);
            }
        }

        public void Add(AgentInfo agentInfo)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            string enableStr;
            if (agentInfo.Enable) enableStr = "true"; else enableStr = "false";

            // Запрос на добавление данных с плейсхолдерами для параметров
            string querySQL = string.Format("INSERT INTO metricsagents(agentaddress, enable) VALUES('{0}', {1})", agentInfo.AgentAddress, enableStr);
            connection.Execute(querySQL);
            if (!_values.ContainsKey(agentInfo.AgentId))
                _values.Add(agentInfo.AgentId, agentInfo);

        }

        public void EnableAgentById(int agentId)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("UPDATE metricsagents SET enable = 1 WHERE AgentId = @id",
            new
            {
                id = agentId
            });
        }

        public void DisableAgentById(int agentId)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("UPDATE metricsagents SET enable = 0 WHERE AgentId = @id",
            new
            {
                id = agentId
            });
        }

        public AgentInfo[] Get()
        {

            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            List<AgentInfo> metricsAgents = connection.Query<AgentInfo>("SELECT * FROM metricsagents").ToList();
            _values = new Dictionary<int, AgentInfo>();
            foreach (AgentInfo agentInfo in metricsAgents)
            {
                if (!_values.ContainsKey(agentInfo.AgentId))
                    _values.Add(agentInfo.AgentId, agentInfo);
            }

            return _values.Values.ToArray();
        }

        public Dictionary<int, AgentInfo> Values
        {
            get { return _values; }
            set { _values = value; }
        }
    }
}
