using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using SpaceGeApp.Core.Common.Abstract;
using SpaceGeApp.Core.Services;
namespace SpaceGeApp.Scheduler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private const string _scheduleExpression = "30 19 * * FRI";
        private SchedulerService _schedulerService;
        public Worker(ILogger<Worker> logger,  SchedulerService schedulerService)
        {
            _schedule = CrontabSchedule.Parse(_scheduleExpression);
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            _schedulerService = schedulerService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                   await _schedulerService.ScheduleEmailAsync();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
