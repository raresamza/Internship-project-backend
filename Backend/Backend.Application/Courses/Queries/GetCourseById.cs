using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Queries;

public record GetCourseById(int courseId) : IRequest<CourseDto>;

public class GetCourseByIdHandler : IRequestHandler<GetCourseById, CourseDto>
{

    public readonly ICourseRepository _courseRepository;

    public GetCourseByIdHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public Task<CourseDto> Handle(GetCourseById request, CancellationToken cancellationToken)
    {
        var course = _courseRepository.GetById(request.courseId);
        if (course == null)
        {
            throw new TeacherNotFoundException($"The teacher witrh id: {request.courseId} was not found!");
        }

        return Task.FromResult(CourseDto.FromCourse(course));
    }
}
