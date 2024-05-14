using Backend.Domain.Models;
using Backend.Application.Students.Responses;
using MediatR;
using Backend.Application.Abstractions;
using AutoMapper;
using Backend.Application.Schools.Update;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Students.Create;


public record CreateStudent(string parentEmail, string parentName, int age, int phoneNumber, string name, string address) : IRequest<StudentDto>;

public class CreateStudentHandler : IRequestHandler<CreateStudent, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateStudentHandler> _logger;
    public CreateStudentHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateStudentHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    //Student rares = new Student("mail@mail.com", "Adi", 11, 11111111, "Rares", "deva");
    public async Task<StudentDto> Handle(CreateStudent request, CancellationToken cancellationToken)
    {
        try
        {
            var student = new Student() { Age = request.age, ParentEmail = request.parentEmail, ParentName = request.parentName, PhoneNumber = request.phoneNumber, Name = request.name, Address = request.address};

            var createdStudent = await _unitOfWork.StudentRepository.Create(student);
            _logger.LogInformation($"Action in students at: {DateTime.Now.TimeOfDay}");

            //return StudentDto.FromStudent(createdStudent);
            return _mapper.Map<StudentDto>(student);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in students at: {DateTime.Now.TimeOfDay}");
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
