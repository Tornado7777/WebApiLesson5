using FluentMigrator.Runner;
using MetricsManager.Controllers;
using MetricsManager.Converters;
using MetricsManager.Models;
using MetricsManager.Services;
using MetricsManager.Services.Impl;
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
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MetricsManager
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
            TimeSpan timeStart = TimeSpan.FromMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                // Добавляем поддержку SQLite
                .AddSQLite()
                .WithGlobalConnectionString(Configuration.GetSection("Settings:DatabaseOptions:ConnectionString").Value)
                .ScanIn(typeof(Startup).Assembly).For.Migrations())
                .AddLogging(lb => lb
                .AddFluentMigratorConsole());

            services.AddHttpClient(); // IHttpClientFactory

            services.AddHttpClient<IMetricsAgentClient,
                MetricsAgentClient>().AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {


                }));



            services.AddSingleton<AgentPool>()
                .Configure<DatabaseOptions>(options =>
                {
                    Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
                });
            services.AddSingleton<AgentsController>(); 

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "API сервиса агента сбора метрик",
                    Description = "Здесь можно поиграть с api нашего сервиса",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Famaly Name",
                        Email = string.Empty,
                        Url = new Uri("https://www.google.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Можно указать, под какой лицензией всё опубликовано",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();

                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString(timeStart.ToString("dd\\.hh\\:mm\\:ss"))
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsManager v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            migrationRunner.MigrateUp();
        }
    }
}
