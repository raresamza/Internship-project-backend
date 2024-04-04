using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Update;

public record UpdateStudent(int studentId, Domain.Models.Student student) : IRequest<StudentDto>;
public class UpdateStudentHandler : IRequestHandler<UpdateStudent, StudentDto>
{
    private readonly IStudentRepository _studentRepository;

    public UpdateStudentHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task<StudentDto> Handle(UpdateStudent request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        if (student == null)
        {
            throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
        }

        var newStudent = _studentRepository.UpdateStudent(request.student, student.ID);

        return Task.FromResult(StudentDto.FromStudent(newStudent));
    }
}
