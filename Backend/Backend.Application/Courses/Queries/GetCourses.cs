using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Queries;

public record GetCourses() : IRequest<List<CourseDto>>;


public class GetCoursesHandler : IRequestHandler<GetCourses, List<CourseDto>>
{

    public readonly IUnitOfWork _unitOfWork;

    public GetCoursesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CourseDto>> Handle(GetCourses request, CancellationToken cancellationToken)
    {
        var courses = await _unitOfWork.CourseRepository.GetAll();
        return courses.Select(course => CourseDto.FromCourse(course)).ToList();
    }
}
