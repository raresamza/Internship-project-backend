using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Teachers.Queries;

public record GetTeacherSchedule(int TeacherId) : IRequest<byte[]>;

public class GetTeacherScheduleHandler : IRequestHandler<GetTeacherSchedule, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulePdfBuilder _pdfBuilder;
    private readonly ILogger<GetTeacherScheduleHandler> _logger;

    public GetTeacherScheduleHandler(
        IUnitOfWork unitOfWork,
        ISchedulePdfBuilder pdfBuilder,
        ILogger<GetTeacherScheduleHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _pdfBuilder = pdfBuilder;
        _logger = logger;
    }

    public async Task<byte[]> Handle(GetTeacherSchedule request, CancellationToken cancellationToken)
    {
        var teacher = await _unitOfWork.TeacherRepository.GetById(request.TeacherId);
        if (teacher == null)
            throw new Exception("Teacher not found.");

        var schedule = _unitOfWork.ScheduleRepository.GenerateSchedule();

        var filtered = schedule
            .Where(e => e.Course.TeacherId == request.TeacherId)
            .ToList();

        return _pdfBuilder.Build(filtered, $"Schedule for {teacher.Name}");
    }
}