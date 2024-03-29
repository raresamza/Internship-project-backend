using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;

public record GetStudentById(int studentId) : IRequest<StudentDto>;



public class GetStudentByIdHandler : IRequestHandler<GetStudentById, StudentDto>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentByIdHandler(IStudentRepository studentRepository) 
    {
        _studentRepository = studentRepository;
    }

    public Task<StudentDto> Handle(GetStudentById request, CancellationToken cancellationToken)
    {
        var student=_studentRepository.GetById(request.studentId);
        if (student == null)
        {
            throw new ApplicationException("Student not found");
        }

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
