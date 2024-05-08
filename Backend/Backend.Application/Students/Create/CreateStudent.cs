using Backend.Domain.Models;
using Backend.Application.Students.Responses;
using MediatR;
using Backend.Application.Abstractions;

namespace Backend.Application.Students.Create;


public record CreateStudent(string parentEmail, string parentName, int age, int phoneNumber, string name, string address) : IRequest<StudentDto>;

public class CreateStudentHandler : IRequestHandler<CreateStudent, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;
    public CreateStudentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    //Student rares = new Student("mail@mail.com", "Adi", 11, 11111111, "Rares", "deva");
    public async Task<StudentDto> Handle(CreateStudent request, CancellationToken cancellationToken)
    {
        try
        {
            var student = new Student() { Age = request.age, ParentEmail = request.parentEmail, ParentName = request.parentName, PhoneNumber = request.phoneNumber, Name = request.name, Address = request.address };

            var createdStudent = await _unitOfWork.StudentRepository.Create(student);
            return StudentDto.FromStudent(createdStudent);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    //private int GetNextId()
    //{
    //    return _studentRepository.GetLastId();
    //}
}
