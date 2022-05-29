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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {



        #region Services

        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly ILogger<HddMetricsController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public HddMetricsController(

            IMapper mapper,
            ILogger<HddMetricsController> logger,
            IHddMetricsRepository hddMetricsRepository)
        {

            _mapper = mapper;
            _logger = logger;
            _hddMetricsRepository = hddMetricsRepository;
        }

        #endregion



        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _hddMetricsRepository.GetAll();
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
                response.Metrics.Add(_mapper.Map<HddMetricDto>(metric));

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
        public IActionResult GetHddMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _hddMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new HddMetricDto
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

