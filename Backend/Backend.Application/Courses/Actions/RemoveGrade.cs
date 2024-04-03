using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Students.Responses;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Actions;
public record RemoveGrade(int studentId, int courseId, int grade) : IRequest<StudentDto>;

public class RemoveGradeHandler : IRequestHandler<RemoveGrade, StudentDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;

    public RemoveGradeHandler(ICourseRepository courseRepository, IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }
    public Task<StudentDto> Handle(RemoveGrade request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var course = _courseRepository.GetById(request.courseId);

        if (student == null)
        {
            throw new StudentNotFoundException($"Student with ID: {request.studentId} could not be found");
        }
        if (course == null)
        {
            throw new NullCourseException($"Could not found course with id: {request.courseId}");
        }

        _studentRepository.RemoveGrade(student,course,request.grade);
        _studentRepository.UpdateStudent(student, student.ID);
        _courseRepository.UpdateCourse(course, course.ID);

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
