using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Actions;

public record EnrollIntoCourse(int studentId, int courseId) : IRequest<StudentDto>;

internal class EntollIntoCourseHandler : IRequestHandler<EnrollIntoCourse, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EntollIntoCourseHandler> _logger;
    public EntollIntoCourseHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EntollIntoCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentDto> Handle(EnrollIntoCourse request, CancellationToken cancellationToken)
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
            if (course.StudentCourses.Any(sc => sc.Student == student))
            {
                throw new StudentException($"Student {student?.Name} is already enrolled into this course");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.StudentRepository.EnrollIntoCourse(student, course);
            //await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            //_courseRepository.UpdateCourse(course,course.ID);
            await _unitOfWork.CommitTransactionAsync();
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
