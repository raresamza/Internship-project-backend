using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Queries;

public record GetClassroomById(int classroomId): IRequest<ClassroomDto>;
public class GetClassroomByIdHandler : IRequestHandler<GetClassroomById, ClassroomDto>
{

    private readonly IClassroomRepository _classroomRepository;

    public GetClassroomByIdHandler(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    Task<ClassroomDto> IRequestHandler<GetClassroomById, ClassroomDto>.Handle(GetClassroomById request, CancellationToken cancellationToken)
    {
        var classroom=_classroomRepository.GetById(request.classroomId);

        if (classroom == null)
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
}
