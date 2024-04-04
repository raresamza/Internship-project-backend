using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;

public record GetStudentByName(string studentName):IRequest<StudentDto>;
public class GetStudentByNameHandler : IRequestHandler<GetStudentByName, StudentDto>
{

    private readonly IStudentRepository _studentRepository;

    public GetStudentByNameHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task<StudentDto> Handle(GetStudentByName request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetByName(request.studentName);
        if (student == null)
        {
            throw new ApplicationException("Student not found");
        }

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
