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

namespace Backend.Application.Courses.Actions;

public record AddGrade(int studentId, int courseId, int grade) : IRequest<StudentDto>;


public class AddGradeHandler : IRequestHandler<AddGrade, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddGradeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            return StudentDto.FromStudent(student);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
