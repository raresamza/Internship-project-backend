using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Exceptions.CourseException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Delete;

public record DeleteCourse(int courseId) : IRequest<CourseDto>;

public class DeleteCourseHandler : IRequestHandler<DeleteCourse, CourseDto>
{

    private readonly ICourseRepository  _courseRepository;

    public DeleteCourseHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public Task<CourseDto> Handle(DeleteCourse request, CancellationToken cancellationToken)
    {
        var course=_courseRepository.GetById(request.courseId);
        if(course == null)
        {
            throw new NullCourseException($"The course with id: {request.courseId} was not found");
        }

        //_courseRepository.DeleteCourse(course);

        return Task.FromResult(CourseDto.FromCourse(course));
    }
}
