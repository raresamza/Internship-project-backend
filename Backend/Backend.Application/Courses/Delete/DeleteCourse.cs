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

    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseDto> Handle(DeleteCourse request, CancellationToken cancellationToken)
    {

        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            if (course == null)
            {
                throw new NullCourseException($"The course with id: {request.courseId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.CourseRepository.DeleteCourse(course);
            await _unitOfWork.CommitTransactionAsync();
            return CourseDto.FromCourse(course);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
