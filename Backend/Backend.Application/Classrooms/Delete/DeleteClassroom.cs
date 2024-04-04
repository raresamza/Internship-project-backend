using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Delete;

public record DeleteClassroom(int classroomId) : IRequest<ClassroomDto>;
public class DeleteClassroomHandler : IRequestHandler<DeleteClassroom, ClassroomDto>
{

    private readonly IClassroomRepository _classroomRepository;

    public DeleteClassroomHandler(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(DeleteClassroom request, CancellationToken cancellationToken)
    {
        var classroom = _classroomRepository.GetById(request.classroomId);

        if (classroom == null)
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        _classroomRepository.Delete(classroom);

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
