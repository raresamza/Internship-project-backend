using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Delete;

public record DeleteTeacher(int teacherId): IRequest<TeacherDto>;
public class DeleteTeacherHandler : IRequestHandler<DeleteTeacher, TeacherDto>
{

    private readonly ITeacherRepository _teacherRepository;

    public DeleteTeacherHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public Task<TeacherDto> Handle(DeleteTeacher request, CancellationToken cancellationToken)
    {
        var teacher = _teacherRepository.GetById(request.teacherId);
        if (teacher == null)
        {
            throw new TeacherNotFoundException($"The teacher with id: {request.teacherId} was not found");
        }

        _teacherRepository.Delete(teacher);

        return Task.FromResult(TeacherDto.FromTeacher(teacher));
    }
}
