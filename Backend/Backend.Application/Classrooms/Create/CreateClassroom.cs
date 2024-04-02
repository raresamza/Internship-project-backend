using Backend.Application.Classrooms.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;

namespace Backend.Application.Classrooms.Create;


public record CreateClassroom(string name) : IRequest<ClassroomDto>;
public class CreateaClassroomHandler : IRequestHandler<CreateClassroom, ClassroomDto>
{
    private readonly IClassroomRepository _classroomRepository;

    public CreateaClassroomHandler(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(CreateClassroom request, CancellationToken cancellationToken)
    {
        var classroom = new Classroom() { Name = request.name, ID = GetNextId() };
        var createdClassroom = _classroomRepository.Create(classroom);
        return Task.FromResult(ClassroomDto.FromClassroom(createdClassroom));
    }

    private int GetNextId()
    {
        return _classroomRepository.GetLastId();
    }
}
