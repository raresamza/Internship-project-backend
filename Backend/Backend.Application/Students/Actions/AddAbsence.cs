using Backend.Application.Absences.Create;
using Backend.Application.Abstractions;
using Backend.Application.Students.Create;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backend.Application.Students.Actions;

public record AddAbsence(int studentId,int absenceId) : IRequest<StudentDto>;
internal class AddAbsenceHandler : IRequestHandler<AddAbsence, StudentDto>
{

    private readonly IStudentRepository _studentRepository;
    private readonly IAbsenceRepository _absenceRepository;

    public AddAbsenceHandler(IStudentRepository studentRepository, IAbsenceRepository absenceRepository)
    {
        _studentRepository = studentRepository;
        _absenceRepository = absenceRepository;
    }

    public Task<StudentDto> Handle(AddAbsence request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var absence=_absenceRepository.GetById(request.absenceId);
        if(student == null)
        {
            throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
        }
        if (absence == null)
        {
            throw new NullCourseException($"Course with id: {request.absenceId} was not found");
        }



        _studentRepository.AddAbsence(student, absence);
        _studentRepository.UpdateStudent(student, student.ID);
        return Task.FromResult(StudentDto.FromStudent(student));


        
    }
}
