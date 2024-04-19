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
using Backend.Domain.Exceptions.SchoolException;

namespace Backend.Application.Classrooms.Create;


public record CreateClassroom(string name,int schoolId) : IRequest<ClassroomDto>;
public class CreateaClassroomHandler : IRequestHandler<CreateClassroom, ClassroomDto>
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly ISchoolRepository _schoolRepository;

    public CreateaClassroomHandler(IClassroomRepository classroomRepository, ISchoolRepository schoolRepository)
    {
        _classroomRepository = classroomRepository;
        _schoolRepository = schoolRepository;
    }

    public Task<ClassroomDto> Handle(CreateClassroom request, CancellationToken cancellationToken)
    {
        var school= _schoolRepository.GetById(request.schoolId);
        if(school == null)
        {
            throw new SchoolNotFoundException($"School with id {request.schoolId} was not found");
        }
        var classroom = new Classroom() { Name = request.name,SchoolId=request.schoolId,School= school};
        var createdClassroom = _classroomRepository.Create(classroom);
        return Task.FromResult(ClassroomDto.FromClassroom(createdClassroom));
    }
}
