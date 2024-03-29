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
    private readonly ITeacherRepository _teacherRepository;

    public CreateTeacherHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }
    public Task<TeacherDto> Handle(CreateTeacher request, CancellationToken cancellationToken)
    {
        var teacher = new Teacher() { Address = request.Address, Subject = request.Subject, Age = request.Age, PhoneNumber = request.PhoneNumber, Name = request.Name,ID=GetNextId()};
        var createdTeacher = _teacherRepository.Create(teacher);
        return Task.FromResult(TeacherDto.FromTeacher(createdTeacher));
    }

    private int GetNextId()
    {
        return _teacherRepository.GetLastId();
    }
}
