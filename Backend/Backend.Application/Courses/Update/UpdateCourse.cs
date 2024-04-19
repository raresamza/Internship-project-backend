using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Update;

public record UpdateCourse(int courseId,Course course) : IRequest<CourseDto>;
public class UpdateCourseHandler : IRequestHandler<UpdateCourse, CourseDto>
{

    private readonly ICourseRepository _courseRepository;

    public UpdateCourseHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public Task<CourseDto> Handle(UpdateCourse request, CancellationToken cancellationToken)
    {
        var course = _courseRepository.GetById(request.courseId);
        if (course == null)
        {
            throw new NullCourseException($"The course with id: {request.courseId} was not found");
        }
        //var newCourse=_courseRepository.UpdateCourse(request.course, course.ID);

        //return Task.FromResult(CourseDto.FromCourse(newCourse));
        return null;
    }
}
