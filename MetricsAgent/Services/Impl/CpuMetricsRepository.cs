using Dapper;
using MetricsAgent.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.Extensions.Options;

namespace MetricsAgent.Services.Impl
{



    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        //private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public CpuMetricsRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(CpuMetric item)
        {
           
            DatabaseOptions databaseOptions = _databaseOptions.Value;
            using var connection = new SQLiteConnection(databaseOptions.ConnectionString);
            // Запрос на добавление данных с плейсхолдерами для параметров
            connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
            // Анонимный объект с параметрами запроса
            new
            {
                // Value подставится на место "@value" в строке запроса
                // Значение запишется из поля Value объекта item
                value = item.Value,
                // Записываем в поле time количество секунд
                time = item.Time
            });
        }
        public void Delete(int id)
        {
            
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("DELETE FROM cpumetrics WHERE id=@id",new { id = id });
        }
        public void Update(CpuMetric item)
        {
            
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            connection.Execute("UPDATE cpumetrics SET value = @value, time = @time WHERE id = @id",
            new
            {
                value = item.Value,
                time = item.Time,
                id = item.Id
            });
        }
        public IList<CpuMetric> GetAll()
        {
            
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            //Id, Time, Value
            List<CpuMetric> metrics = connection.Query<CpuMetric>("SELECT * FROM cpumetrics").ToList();
            return metrics;
        }
        public CpuMetric GetById(int id)
        {
            
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            CpuMetric metric = connection.QuerySingle<CpuMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id = @id",
            new { id = id });
            return metric;
        }

        /// <summary>
        /// Получение данных по нагрузке на ЦП за период
        /// </summary>
        /// <param name="timeFrom">Время начала периода</param>
        /// <param name="timeTo">Время окончания периода</param>
        /// <returns></returns>
        public IList<CpuMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
           
            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);
            string table = "cpumetrics";
            List<CpuMetric> metrics = connection.Query<CpuMetric>($"SELECT * FROM {table} where time >= @timeFrom and time <= @timeTo",
                new { timeFrom = timeFrom.TotalSeconds, timeTo = timeTo.TotalSeconds }).ToList();
            return metrics;
        }
    }
}
