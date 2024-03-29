using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Create;


public record CreateCourse(string Name, Subject Subject) : IRequest<CourseDto>;

public class CreateCourseHandler : IRequestHandler<CreateCourse, CourseDto>
{

    private readonly ICourseRepository _courseRepository;

    public CreateCourseHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public Task<CourseDto> Handle(CreateCourse request, CancellationToken cancellationToken)
    {
        var course = new Course() { Name = request.Name, Subject = request.Subject };
        var createdCourse = _courseRepository.Create(course);
        return Task.FromResult(CourseDto.FromCourse(createdCourse));

    }
}
