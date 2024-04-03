using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record RemoveTeacherFromClassroom(int teacherId,int classroomId) : IRequest<ClassroomDto>;
public class RemoveTeacherFromClassroomHandler : IRequestHandler<RemoveTeacherFromClassroom, ClassroomDto>
{

    private readonly ITeacherRepository _teacherRepository;
    private readonly IClassroomRepository _classroomRepository;

    public RemoveTeacherFromClassroomHandler(ITeacherRepository teacherRepository, IClassroomRepository classroomRepository)
    {
        _teacherRepository = teacherRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(RemoveTeacherFromClassroom request, CancellationToken cancellationToken)
    {
        var teacher=_teacherRepository.GetById(request.teacherId);
        var classroom=_classroomRepository.GetById(request.classroomId);

        if (teacher == null)
        {
            throw new TeacherNotFoundException($"Teacher with id: {request.teacherId} was not found");
        }
        if (classroom == null)
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        _classroomRepository.RemoveTeacher(teacher,classroom);
        _classroomRepository.UpdateClassroom(classroom,classroom.ID);

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
}
