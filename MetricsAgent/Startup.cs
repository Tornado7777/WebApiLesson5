using MetricsAgent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Converters;
using NLog.Extensions.Logging;
using MetricsAgent.Models;
using MetricsAgent.Services.Impl;
using AutoMapper;
using FluentMigrator.Runner;
using Quartz.Spi;
using MetricsAgent.Jobs;
using Quartz;
using Quartz.Impl;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                // ��������� ��������� SQLite
                .AddSQLite()
                .WithGlobalConnectionString(Configuration.GetSection("Settings:DatabaseOptions:ConnectionString").Value)
                .ScanIn(typeof(Startup).Assembly).For.Migrations())
                .AddLogging(lb => lb
                .AddFluentMigratorConsole());

            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<CpuMetricJob>();
            // https://www.freeformatter.com/cron-expression-generator-quartz.html
            services.AddSingleton(new JobSchedule(
                typeof(CpuMetricJob),
                "0/5 * * ? * * *"));

            services.AddHostedService<QuartzHostedService>();

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new
                MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            // ��������� TimeSpan �� ������ ������ � ��������
            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

           // ConfigureSqlLiteConnection(services);

            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>()
                .Configure<DatabaseOptions>(options =>
                {
                    Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
                });

            services.AddTransient<SampleData>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });

                // ��������� TimeSpan �� ������ swagger
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("0:00:00:00")
                });
            });
        }

        //private void ConfigureSqlLiteConnection(IServiceCollection services)
        //{
        //    const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100;";
        //    var connection = new SQLiteConnection(connectionString);
        //    connection.Open();
        //    PrepareSchema(connection);
        //}

        //private void PrepareSchema(SQLiteConnection connection)
        //{
        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        // ����� ����� ����� ������� ��� ����������
        //        // ������� ������� � ���������, ���� ��� ���� � ���� ������
        //        command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
        //        // ���������� ������ � ���� ������
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE cpumetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsAgent v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // TODO: �������� ������� [���� 5, ����� 3]
            // ����������� FluentMigrator

            migrationRunner.MigrateUp();
        }
    }
}
