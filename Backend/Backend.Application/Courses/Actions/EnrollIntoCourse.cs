using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Actions;

public record EnrollIntoCourse(int studentId, int courseId) : IRequest<StudentDto>;

internal class EntollIntoCourseHandler : IRequestHandler<EnrollIntoCourse, StudentDto>
{
    private IUnitOfWork _unitOfWork;

    public EntollIntoCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.StudentRepository.EnrollIntoCourse(student, course);
            await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
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
