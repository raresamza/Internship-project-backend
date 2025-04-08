using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Students.Queries;


public record GetStudentSchedule(int StudentId) : IRequest<byte[]>;

public class GetStudentScheduleHandler : IRequestHandler<GetStudentSchedule, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulePdfBuilder _pdfBuilder;
    private readonly ILogger<GetStudentScheduleHandler> _logger;

    public GetStudentScheduleHandler(
        IUnitOfWork unitOfWork,
        ISchedulePdfBuilder pdfBuilder,
        ILogger<GetStudentScheduleHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _pdfBuilder = pdfBuilder;
        _logger = logger;
    }

    public async Task<byte[]> Handle(GetStudentSchedule request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentRepository.GetById(request.StudentId);


        _logger.LogWarning(student.ToString());

        if (student == null)
            throw new Exception("Student not found.");

        if (student.StudentCoruses == null || !student.StudentCoruses.Any())
        {
            _logger.LogWarning($"Student '{student.Name}' is not enrolled in any courses.");
            return _pdfBuilder.Build(new List<ScheduleEntry>(), $"Schedule for {student.Name} (No Courses)");
        }

        var courseIds = student.StudentCoruses
            .Select(sc => sc.CourseId)
            .ToList();

        _logger.LogInformation($"Student '{student.Name}' is enrolled in courses: {string.Join(", ", courseIds)}");

        var schedule = _unitOfWork.ScheduleRepository.GenerateSchedule();

        _logger.LogInformation($"Generated {schedule.Count} schedule entries total.");
        _logger.LogInformation("Scheduled Course IDs:");
        foreach (var entry in schedule)
        {
            _logger.LogInformation($"  → Course ID: {entry.Course.ID} in {entry.Classroom.Name} at {entry.TimeSlot.Day} {entry.TimeSlot.StartTime}");
        }

        var filteredSchedule = schedule
            .Where(e => courseIds.Contains(e.Course.ID))
            .ToList();

        _logger.LogInformation($"Filtered schedule contains {filteredSchedule.Count} entries.");

        return _pdfBuilder.Build(filteredSchedule, $"Schedule for {student.Name}");
    }

}

