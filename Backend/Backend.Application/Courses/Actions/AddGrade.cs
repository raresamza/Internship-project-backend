using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Students.Responses;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.CourseException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using AutoMapper;
using Backend.Application.Classrooms.Actions;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Courses.Actions;

public record AddGrade(int studentId, int courseId, int grade) : IRequest<StudentDto>;


public class AddGradeHandler : IRequestHandler<AddGrade, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddGradeHandler> _logger;
    private readonly IMailService _mailService;
    public AddGradeHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddGradeHandler> logger, IMailService mailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _mailService = mailService;
    }

    public async Task<StudentDto> Handle(AddGrade request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);

            if (student == null)
            {
                throw new StudentNotFoundException($"Student with ID: {request.studentId} could not be found");
            }
            if (course == null)
            {
                throw new NullCourseException($"Could not found course with id: {request.courseId}");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.StudentRepository.AddGrade(request.grade, student, course);
            //_studentRepository.UpdateStudent(student,student.ID);
            //_courseRepository.UpdateCourse(course,course.ID);
            await _unitOfWork.CommitTransactionAsync();
            await _unitOfWork.NotificationRepository.AddNotificationAsync(new Notification
            {
                StudentId = request.studentId,
                Title = "New Grade Assigned",
                Message = $"You received a grade of {request.grade} for coruse {course.Name}.",
                Type = NotificationType.Grade,
                SentByEmail = await _mailService.SendSimpleEmailAsync(student.ParentEmail, "New Grade Assigned", $"You received a grade of {request.grade} for coruse {course.Name}.") // Optional fallback
            });
            _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
            return StudentDto.FromStudent(student);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in course at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
