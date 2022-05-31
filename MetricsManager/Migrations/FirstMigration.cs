using FluentMigrator;

namespace MetricsManager.Migrations
{

    [Migration(1)]
    public class FirstMigration : Migration
    {

        public override void Up()
        {
            Create.Table("metricsagents").WithColumn("AgentId").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentaddress").AsString().WithColumn("enable").AsBoolean();
           
        }


        public override void Down()
        {
            Delete.Table("metricsagent");
            
        }

        
    }
}
