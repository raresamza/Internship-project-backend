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

public record UpdateCourse(int courseId, Course course) : IRequest<CourseDto>;
public class UpdateCourseHandler : IRequestHandler<UpdateCourse, CourseDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseDto> Handle(UpdateCourse request, CancellationToken cancellationToken)
    {

        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            if (course == null)
            {
                throw new NullCourseException($"The course with id: {request.courseId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            var newCourse = await _unitOfWork.CourseRepository.UpdateCourse(request.course, course.ID);
            await _unitOfWork.CommitTransactionAsync();
            return CourseDto.FromCourse(newCourse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
