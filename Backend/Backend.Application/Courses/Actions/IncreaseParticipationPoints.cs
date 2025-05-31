using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;


public record IncreaseParticipationPoints(int StudentId, int CourseId) : IRequest<Unit>;

public class IncreaseParticipationPointsHandler : IRequestHandler<IncreaseParticipationPoints, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IncreaseParticipationPointsHandler> _logger;

    public IncreaseParticipationPointsHandler(IUnitOfWork unitOfWork, ILogger<IncreaseParticipationPointsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(IncreaseParticipationPoints request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.StudentId);
            var course = await _unitOfWork.CourseRepository.GetById(request.CourseId);



            if (student == null || course == null)
                throw new ArgumentException("Student or Course not found.");

            var studentCourse = student.StudentCoruses
                .FirstOrDefault(sc => sc.CourseId == request.CourseId);

            if (studentCourse == null)
                throw new InvalidOperationException("Student is not enrolled in the course.");

            await _unitOfWork.BeginTransactionAsync();

            studentCourse.ParticipationPoints += 1;
            await _unitOfWork.SaveAsync();

            await _unitOfWork.CommitTransactionAsync();


            _logger.LogInformation($"✅ Increased participation points for StudentId={request.StudentId}, CourseId={request.CourseId}");

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Error increasing participation points: {ex.Message}");
            throw;
        }
    }
}
