using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Delete;

public record DeleteStudent(int studentId):IRequest<StudentDto>;
public class DeleteStudentHandler : IRequestHandler<DeleteStudent, StudentDto>
{

    private readonly IStudentRepository _studentRepository;

    public DeleteStudentHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public Task<StudentDto> Handle(DeleteStudent request, CancellationToken cancellationToken)
    {
        var student=_studentRepository.GetById(request.studentId);
        if(student == null)
        {
            throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
        }
        _studentRepository.Delete(student);

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
