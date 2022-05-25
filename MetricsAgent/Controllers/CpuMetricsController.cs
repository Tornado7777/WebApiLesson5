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
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {

       

        #region Services

        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly ILogger<CpuMetricsController> _logger;
        private readonly IMapper _mapper;
        //private readonly SampleData _sampleData;
        //private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructors

        public CpuMetricsController(
            //IServiceProvider serviceProvider,
            //SampleData sampleData,
            IMapper mapper,
            ILogger<CpuMetricsController> logger,
            ICpuMetricsRepository cpuMetricsRepository)
        {
            //_sampleData = sampleData;
            //_serviceProvider = serviceProvider;
            _mapper = mapper;
            _logger = logger;
            _cpuMetricsRepository = cpuMetricsRepository;
        }

        #endregion

        

        // TODO: Домашняя работа
        // 2. Настройте AutoMapper для остальных объектов в приложениях: преобразование из DTO в
        // модели базы и обратно.

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _cpuMetricsRepository.GetAll();
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };

            foreach (var metric in metrics)
                response.Metrics.Add(_mapper.Map<CpuMetricDto>(metric));

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
        public IActionResult GetCpuMetrics(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _cpuMetricsRepository.GetByTimePeriod(fromTime, toTime);
            var response = new AllCpuMetricsResponse()
            {
                Metrics = new List<CpuMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(new CpuMetricDto
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
