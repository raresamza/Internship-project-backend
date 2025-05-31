using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using Backend.Application.Students.Actions;
using Backend.Application.Students.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Services;

public class WeeklyReportService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WeeklyReportService> _logger;

    public WeeklyReportService(IServiceProvider serviceProvider, ILogger<WeeklyReportService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var nextRunTime = DateTime.Today.AddDays(7); // Run weekly
            var delay = nextRunTime - DateTime.Now;
            await Task.Delay(delay, stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var students = await mediator.Send(new GetAllStudents());

                foreach (var student in students)
                {
                    var report = await mediator.Send(new GenerateStudentReport(student.ID));
                    var email = student.ParentEmail;
                    var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                    await mailService.SendGradePdfAsync(email, new MemoryStream(report));
                }

                _logger.LogInformation("Weekly reports sent to parents/students.");
            }
        }
    }
}