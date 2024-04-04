using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Update;

public record UpdateTeacher(int teacherId,Teacher teacher) : IRequest<TeacherDto>;
public class UpdateTeacherHandler : IRequestHandler<UpdateTeacher, TeacherDto>
{

    private readonly ITeacherRepository _teacherRepository;

    public UpdateTeacherHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public Task<TeacherDto> Handle(UpdateTeacher request, CancellationToken cancellationToken)
    {
        var teacher = _teacherRepository.GetById(request.teacherId);

        if(teacher == null)
        {
            throw new TeacherNotFoundException($"The teacher with id: {request.teacher} was not found");
        }

        var newTeacher = _teacherRepository.UpdateTeacher(request.teacher, teacher.ID);
        return Task.FromResult(TeacherDto.FromTeacher(newTeacher));
    }
}
