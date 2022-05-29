using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {



        #region Services

        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public NetworkMetricsController(

            IMapper mapper,
            ILogger<NetworkMetricsController> logger,
            INetworkMetricsRepository networkMetricsRepository)
        {

            _mapper = mapper;
            _logger = logger;
            _networkMetricsRepository = networkMetricsRepository;
        }

        #endregion



        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _networkMetricsRepository.GetAll();
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };

            foreach (var metric in metrics)
                response.Metrics.Add(_mapper.Map<NetworkMetricDto>(metric));

            return Ok(response);
        }



        // TODO: Домашнее задание [Урок 5, пункт 2]
        // Реализуйте контроллеры, которые будут отдавать данные по собираемым метрикам.

        /// <summary>
        /// Получить статистику по нагрузке на ЦП за период
        /// </summary>
        /// <param name="fromTime">Время начала периода</param>
        /// <param name="toTime">Время окончания периода</param>
        /// <returns></returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetNetworkMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _networkMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetricDto
                {
                    Time = TimeSpan.FromSeconds(metric.Time),
                    Value = metric.Value,
                    Id = metric.Id
                });
            }
            return Ok(response);
        }

    }
}

