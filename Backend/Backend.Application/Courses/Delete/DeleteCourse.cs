using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Actions;
using Backend.Application.Courses.Response;
using Backend.Exceptions.CourseException;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCourseHandler> _logger;
    public DeleteCourseHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
            return CourseDto.FromCourse(course);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in course at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
