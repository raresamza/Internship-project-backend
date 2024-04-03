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

namespace Backend.Application.Students.Actions;

public record MotivateAbsence(int studentId, int absenceId,int courseId) : IRequest<StudentDto>;

public class MotivateAbsenceHandler : IRequestHandler<MotivateAbsence, StudentDto>
{

    private readonly IStudentRepository _studentRepository;
    private readonly IAbsenceRepository _absenceRepository;
    private readonly ICourseRepository _courseRepository;

    public MotivateAbsenceHandler(IStudentRepository studentRepository, IAbsenceRepository absenceRepository, ICourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _absenceRepository = absenceRepository;
        _courseRepository = courseRepository;
    }

    public Task<StudentDto> Handle(MotivateAbsence request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var absence = _absenceRepository.GetById(request.absenceId);
        var course = _courseRepository.GetById(request.courseId);

        if(course == null)
        {
            throw new NullCourseException($"The course with id: {request.courseId} was not found");
        }
        if (student == null)
        {
            throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
        }
        if (absence == null)
        {
            throw new NullCourseException($"Course with id: {request.absenceId} was not found");
        }

        _studentRepository.MotivateAbsence(absence.Date,course,student);
        _studentRepository.UpdateStudent(student, student.ID);
        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
