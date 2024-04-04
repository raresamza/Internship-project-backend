using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;

public record GetAbsenceByDateAndCourse(DateTime Date,int courseId,int studentId): IRequest<AbsenceDto>;
public class GetAbsenceByDateAndCourseHandler : IRequestHandler<GetAbsenceByDateAndCourse, AbsenceDto>
{


    private readonly IAbsenceRepository _absenceRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;

    public GetAbsenceByDateAndCourseHandler(IAbsenceRepository absenceRepository, ICourseRepository courseRepository, IStudentRepository studentRepository)
    {
        _absenceRepository = absenceRepository;
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public Task<AbsenceDto> Handle(GetAbsenceByDateAndCourse request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var course=_courseRepository.GetById(request.courseId);
        if(student == null)
        {
            throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
        }
        if(course == null)
        {
            throw new NullCourseException($"The course with id: {request.courseId} was not found");
        }
        var absence = _absenceRepository.GetByDateAndCourse(request.Date,course,student);
        if (absence == null)
        {
            throw new TeacherNotFoundException($"The absence for the course: {request.courseId}, on date: {request.Date} was not found!");
        }
        return Task.FromResult(AbsenceDto.FromAbsence(absence));
    }
}
