using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.CourseException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record AssignCourseToClassroom(int courseId,int classroomId):IRequest<ClassroomDto>;
public class AssignCourseToClassroomHadnler : IRequestHandler<AssignCourseToClassroom, ClassroomDto>
{

    private readonly ICourseRepository _courseRepository;
    private readonly IClassroomRepository _classroomRepository;

    public AssignCourseToClassroomHadnler(ICourseRepository courseRepository, IClassroomRepository classroomRepository)
    {
        _courseRepository = courseRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(AssignCourseToClassroom request, CancellationToken cancellationToken)
    {
        var course = _courseRepository.GetById(request.courseId);
        var classroom= _classroomRepository.GetById(request.classroomId);
        if (course == null)
        {
            throw new NullCourseException($"Course with id: {request.courseId} was not found");
        }
        if (classroom == null)
        {
            throw new NullClassroomException($"Classroom with id: {request.classroomId} was not found");
        }

        _classroomRepository.AssignCourse(course, classroom);
        _classroomRepository.UpdateClassroom(classroom,classroom.ID);

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
}
