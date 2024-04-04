using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Update;

public record UpdateClassroom(int classroomId,Classroom classroom): IRequest<ClassroomDto>;
public class UpdateClassroomHandler : IRequestHandler<UpdateClassroom, ClassroomDto>
{

    private readonly IClassroomRepository _classroomRepository;

    public UpdateClassroomHandler(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(UpdateClassroom request, CancellationToken cancellationToken)
    {
        var classroom = _classroomRepository.GetById(request.classroomId);

        if (classroom == null)
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        var newClassroom = _classroomRepository.UpdateClassroom(request.classroom, classroom.ID);

        return Task.FromResult(ClassroomDto.FromClassroom(newClassroom));
    }
}
