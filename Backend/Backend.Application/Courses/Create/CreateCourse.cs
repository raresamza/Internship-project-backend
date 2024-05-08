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

    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseDto> Handle(CreateCourse request, CancellationToken cancellationToken)
    {
        try
        {
            var course = new Course() { Name = request.Name, Subject = request.Subject };
            await _unitOfWork.BeginTransactionAsync();
            var createdCourse = await _unitOfWork.CourseRepository.Create(course);
            await _unitOfWork.CommitTransactionAsync();
            return CourseDto.FromCourse(createdCourse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }


    }
}
