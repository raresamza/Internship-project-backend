using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Homeworks.Response;
using Backend.Application.Students.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Homeworks.Actions;


public record GradeStudentHomework(int HomeworkId, int StudentId, int grade) : IRequest<StudentHomeworkDto>;

//public record EnrollIntoCourse(int studentId, int courseId) : IRequest<StudentDto>;

public class GradeStudentHomeworkHandler : IRequestHandler<GradeStudentHomework, StudentHomeworkDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GradeStudentHomeworkHandler> _logger;

    public GradeStudentHomeworkHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GradeStudentHomeworkHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentHomeworkDto> Handle(GradeStudentHomework request, CancellationToken cancellationToken)
    {
        try
        {
            var studentHomework = await _unitOfWork.StudentRepository.GetByStudentAndHomework(request.StudentId, request.HomeworkId);

            if (studentHomework == null)
                throw new Exception($"No student-homework entry found for student {request.StudentId} and homework {request.HomeworkId}");


            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.StudentRepository.GradeHomework(studentHomework, request.grade);
            await _unitOfWork.CommitTransactionAsync();


            _logger.LogInformation($"Graded homework {request.HomeworkId} for student {request.StudentId} with {request.grade}");
            return _mapper.Map<StudentHomeworkDto>(studentHomework);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to grade student homework");
            await _unitOfWork.RollbackTransactionAsync();
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}