using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Homeworks.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
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
    private readonly IMailService _mailService;

    public GradeStudentHomeworkHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GradeStudentHomeworkHandler> logger,
        IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _mailService = mailService;
    }

    public async Task<StudentHomeworkDto> Handle(GradeStudentHomework request, CancellationToken cancellationToken)
    {
        try
        {
            var studentHomework = await _unitOfWork.StudentRepository
                .GetByStudentAndHomework(request.StudentId, request.HomeworkId);
            var student = await _unitOfWork.StudentRepository.GetById(request.StudentId);

            if (student == null)
                throw new StudentNotFoundException($"No student entry found for student {request.StudentId}");

            if (studentHomework == null)
                throw new Exception($"No student-homework entry found for student {request.StudentId} and homework {request.HomeworkId}");

            // Grade the homework directly (no transaction)
            _unitOfWork.StudentRepository.GradeHomework(studentHomework, request.grade);

            // Save notification email first
            var sentByEmail = await _mailService.SendSimpleEmailAsync(
                student.ParentEmail,
                "New Grade Assigned",
                $"You received a grade of {request.grade} for homework #{request.HomeworkId}."
            );

            // Then save notification in the database
            await _unitOfWork.NotificationRepository.AddNotificationAsync(new Notification
            {
                StudentId = request.StudentId,
                Title = "New Grade Assigned",
                Message = $"You received a grade of {request.grade} for homework #{request.HomeworkId}.",
                Type = NotificationType.Grade,
                SentByEmail = sentByEmail
            });

            _logger.LogInformation($"Graded homework {request.HomeworkId} for student {request.StudentId} with {request.grade}");
            return _mapper.Map<StudentHomeworkDto>(studentHomework);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to grade student homework");
            throw;
        }
    }




}