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

public record AddGrade(int studentId,int courseId, int grade) : IRequest<StudentDto>;


public class AddGradeHandler : IRequestHandler<AddGrade, StudentDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;

    public AddGradeHandler(ICourseRepository courseRepository, IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public Task<StudentDto> Handle(AddGrade request, CancellationToken cancellationToken)
    {
        var student=_studentRepository.GetById(request.studentId);
        var course=_courseRepository.GetById(request.courseId);

        if(student == null)
        {
            throw new StudentNotFoundException($"Student with ID: {request.studentId} could not be found");
        }
        if(course == null)
        {
            throw new NullCourseException($"Could not found course with id: {request.courseId}");
        }

        _studentRepository.AddGrade(request.grade,student,course);
        //_studentRepository.UpdateStudent(student,student.ID);
        //_courseRepository.UpdateCourse(course,course.ID);

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
