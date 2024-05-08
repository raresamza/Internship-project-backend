using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Create;

public record CreateTeacher(int Age, int PhoneNumber, string Name, string Address, Subject Subject) : IRequest<TeacherDto>;

public class CreateTeacherHandler : IRequestHandler<CreateTeacher, TeacherDto>
{
    private readonly IUnitOfWork _unitOfWOork;

    public CreateTeacherHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWOork = unitOfWork;
    }
    public async Task<TeacherDto> Handle(CreateTeacher request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = new Teacher() { Address = request.Address, Subject = request.Subject, Age = request.Age, PhoneNumber = request.PhoneNumber, Name = request.Name };

            await _unitOfWOork.BeginTransactionAsync();
            var createdTeacher = await _unitOfWOork.TeacherRepository.Create(teacher);
            await _unitOfWOork.CommitTransactionAsync();
            return TeacherDto.FromTeacher(createdTeacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWOork.RollbackTransactionAsync();
            throw;
        }


    }
}
