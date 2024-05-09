using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Actions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCourseHandler> _logger;
    public CreateCourseHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CourseDto> Handle(CreateCourse request, CancellationToken cancellationToken)
    {
        try
        {
            var course = new Course() { Name = request.Name, Subject = request.Subject };
            await _unitOfWork.BeginTransactionAsync();
            var createdCourse = await _unitOfWork.CourseRepository.Create(course);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
            return CourseDto.FromCourse(createdCourse);
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
