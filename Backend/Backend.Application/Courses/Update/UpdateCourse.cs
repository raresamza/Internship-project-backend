using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Actions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Update;

public record UpdateCourse(int courseId, CourseUpdateDto course) : IRequest<CourseDto>;
public class UpdateCourseHandler : IRequestHandler<UpdateCourse, CourseDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCourseHandler> _logger;
    public UpdateCourseHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
            return CourseDto.FromCourse(newCourse);
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
