using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Queries;


public record GetTeacherById(int Id): IRequest<TeacherDto>;

public class GetTeacherByIdHandler : IRequestHandler<GetTeacherById, TeacherDto>
{

    private readonly ITeacherRepository _teacherRepository;

    public GetTeacherByIdHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public Task<TeacherDto> Handle(GetTeacherById request, CancellationToken cancellationToken)
    {
        var teacher=_teacherRepository.GetById(request.Id);
        if (teacher == null)
        {
            throw new TeacherNotFoundException($"The teacher witrh id: {request.Id} was not found!");
        }

        return Task.FromResult(TeacherDto.FromTeacher(teacher));
    }
}
