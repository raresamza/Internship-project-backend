﻿using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Backend.Application.Courses.Actions;

public record AddStudent(int studentId, int courseId) : IRequest<CourseDto>;
public class AddStudentHandler : IRequestHandler<AddStudent, CourseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddStudentHandler> _logger;
    public AddStudentHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddStudentHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CourseDto> Handle(AddStudent request, CancellationToken cancellationToken)
    {


        try
        {
            var dbCourse = await _unitOfWork.CourseRepository.GetById(request.courseId);
            var dbStudent = await _unitOfWork.StudentRepository.GetById(request.studentId);

            if (dbCourse == null)
            {
                throw new NullCourseException($"The course with id: {request.courseId} was not found");
            }

            if (dbStudent == null)
            {
                throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
            }

            if (dbCourse.StudentCourses.Any(sc => sc.Student == dbStudent))
            {
                throw new StudentException($"Student {dbStudent?.Name} is already enrolled into this course");
            }
            var studentCourse = new StudentCourse { Student = dbStudent, Course = dbCourse, StudentId = dbStudent.ID, CourseId = dbCourse.ID };
            dbCourse.StudentCourses.Add(studentCourse);
            List<int> grades = new List<int>();

            dbStudent.GPAs.Add(new StudentGPA
            {
                StudentId = dbStudent.ID,
                CourseId = dbCourse.ID,
                GPAValue = 0,
                Student = dbStudent,
                Course = dbCourse,
            });
            dbStudent.Grades.Add(new StudentGrade
            {
                StudentId = dbStudent.ID,
                CourseId = dbCourse.ID,
                GradeValues = grades,
                Student = dbStudent,
                Course = dbCourse,
            });
            //_courseRepository.UpdateCourse(dbCourse, dbCourse.ID);
            await _unitOfWork.BeginTransactionAsync();
            //await _unitOfWork.StudentRepository.UpdateStudent(dbStudent, dbStudent.ID);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
            return CourseDto.FromCourse(dbCourse);
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
