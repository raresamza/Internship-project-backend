using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Backend.Application.Abstractions;
//using WebApi.Services;

public class UpcomingDeadlineNotificationService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<UpcomingDeadlineNotificationService> _logger;

    public UpcomingDeadlineNotificationService(IServiceProvider services, ILogger<UpcomingDeadlineNotificationService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                var now = DateTime.UtcNow;
                var tomorrow = now.AddDays(1).Date;

                var homeworksDueTomorrow = await unitOfWork.HomeworkRepository
                    .GetHomeworksDueOnDateAsync(tomorrow); // implement this

                foreach (var homework in homeworksDueTomorrow)
                {
                    foreach (var sh in homework.StudentHomeworks)
                    {
                        if (!sh.IsCompleted && sh.Student.ParentEmail != null)
                        {
                            var subject = $"Reminder: Homework \"{homework.Title}\" is due tomorrow!";
                            var body = $"Dear {sh.Student.Name},\n\nDon't forget to submit your homework titled \"{homework.Title}\" by {homework.Deadline:d}.\n\nBest of luck! 📚";

                            await mailService.SendSimpleEmailAsync(sh.Student.ParentEmail, subject, body);
                        }
                    }
                }

                _logger.LogInformation("Checked and sent deadline notifications at: {Time}", DateTime.Now);
            }

            await Task.Delay(TimeSpan.FromHours(12), stoppingToken); // check twice a day
        }
    }
}

