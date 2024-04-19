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

public record EnrollIntoCourse(int studentId,int courseId) : IRequest<StudentDto>;

internal class EntollIntoCourseHandler : IRequestHandler<EnrollIntoCourse, StudentDto>
{
    private IStudentRepository _studentRepository;
    private ICourseRepository _courseRepository;

    public EntollIntoCourseHandler(IStudentRepository studentRepository, ICourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public Task<StudentDto> Handle(EnrollIntoCourse request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var course=_courseRepository.GetById(request.courseId);

        if (student == null)
        {
            throw new StudentNotFoundException($"Student with ID: {request.studentId} could not be found");
        }
        if (course == null)
        {
            throw new NullCourseException($"Could not found course with id: {request.courseId}");
        }

        _studentRepository.EnrollIntoCourse(student, course);
        _studentRepository.UpdateStudent(student, student.ID);
        //_courseRepository.UpdateCourse(course,course.ID);

        return Task.FromResult(StudentDto.FromStudent(student));


    }
}
