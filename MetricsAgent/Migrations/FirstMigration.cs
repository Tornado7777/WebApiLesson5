using FluentMigrator;

namespace MetricsAgent.Migrations
{

    [Migration(1)]
    public class FirstMigration : Migration
    {

        public override void Up()
        {
            Create.Table("cpumetrics").WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32().WithColumn("Time").AsInt64();
        }


        public override void Down()
        {
            Delete.Table("cpumetrics");
        }

        
    }
}
