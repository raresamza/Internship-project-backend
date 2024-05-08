using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.CourseException;
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

    public readonly IUnitOfWork _unitOfWork;

    public GetCourseByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseDto> Handle(GetCourseById request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
        if (course == null)
        { 
            throw new NullCourseException($"The course with id: {request.courseId} was not found!");
        }

        return CourseDto.FromCourse(course);
    }
}
