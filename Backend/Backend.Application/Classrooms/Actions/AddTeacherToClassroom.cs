using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record AddTeacherToClassroom(int teacherId, int classroomId) : IRequest<ClassroomDto>;

public class AddTeacherToClassroomHandler : IRequestHandler<AddTeacherToClassroom, ClassroomDto>
{

    private readonly ITeacherRepository _teacherRepository;
    private readonly IClassroomRepository _classroomRepository;

    public AddTeacherToClassroomHandler( ITeacherRepository teacherRepository, IClassroomRepository classroomRepository)
    {
        _teacherRepository = teacherRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(AddTeacherToClassroom request, CancellationToken cancellationToken)
    {
        var teacher=_teacherRepository.GetById(request.teacherId);
        var classroom=_classroomRepository.GetById(request.classroomId);

        _classroomRepository.AddTeacher(teacher,classroom);
        _classroomRepository.UpdateClassroom(classroom,classroom.ID);
        _teacherRepository.UpdateTeacher(teacher,teacher.ID);

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
}
