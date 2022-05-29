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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {



        #region Services

        private readonly IRamMetricsRepository _ramMetricsRepository;
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public RamMetricsController(

            IMapper mapper,
            ILogger<RamMetricsController> logger,
            IRamMetricsRepository ramMetricsRepository)
        {

            _mapper = mapper;
            _logger = logger;
            _ramMetricsRepository = ramMetricsRepository;
        }

        #endregion



        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _ramMetricsRepository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));

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
        public IActionResult GetRamMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _ramMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetricDto
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

