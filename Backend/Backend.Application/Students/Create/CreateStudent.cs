using Backend.Domain.Models;
using Backend.Application.Students.Responses;
using MediatR;
using Backend.Application.Abstractions;

namespace Backend.Application.Students.Create;


public record CreateStudent(string parentEmail, string parentName, int age, int phoneNumber, string name, string address) : IRequest<StudentDto>;

public class CreateStudentHandler : IRequestHandler<CreateStudent, StudentDto>
{

    private readonly IStudentRepository _studentRepository;

    public CreateStudentHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    //Student rares = new Student("mail@mail.com", "Adi", 11, 11111111, "Rares", "deva");
    public Task<StudentDto> Handle(CreateStudent request, CancellationToken cancellationToken)
    {

        var student = new Student() { Age = request.age, ParentEmail = request.parentEmail, ParentName = request.parentName, PhoneNumber = request.phoneNumber, Name = request.name, Address = request.address };

        var createdStudent = _studentRepository.Create(student);

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
