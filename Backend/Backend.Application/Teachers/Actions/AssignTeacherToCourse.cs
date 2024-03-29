using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using MediatR;
using System;
using Backend.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.TeacherException;

namespace Backend.Application.Teachers.Actions;

//public void AssignToCourse(int courseID, int teacherID);
public record AssignTeacherToCourse(int courseId, int teacherId) : IRequest<TeacherDto>;

public class AssignTeacherToCourseHandler : IRequestHandler<AssignTeacherToCourse, TeacherDto>
{

    public readonly ITeacherRepository _teacherRepository;
    public readonly ICourseRepository _courseRepository;

    public AssignTeacherToCourseHandler(ITeacherRepository teacherRepository, ICourseRepository courseRepository)
    {
        _teacherRepository = teacherRepository;
        _courseRepository = courseRepository;
    }

    public Task<TeacherDto> Handle(AssignTeacherToCourse request, CancellationToken cancellationToken)
    {
        var teacher = _teacherRepository.GetById(request.teacherId);
        var course = _courseRepository.GetById(request.courseId);
        if (course != null && teacher != null)
        {
            teacher.TaughtCourse = course;
            _teacherRepository.UpdateTeacher(teacher, teacher.ID);
            course.Teacher = teacher;
            _courseRepository.UpdateCourse(course, course.ID);
            return Task.FromResult(TeacherDto.FromTeacher(teacher));
        }
        throw new TeacherNotFoundException("S");
    }
}
