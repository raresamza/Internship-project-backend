using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Response;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Queries;

public record GetCourseById(int courseId) : IRequest<CourseDto>;

public class GetCourseByIdHandler : IRequestHandler<GetCourseById, CourseDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCourseByIdHandler> _logger;
    public GetCourseByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCourseByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CourseDto> Handle(GetCourseById request, CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
        try
        {
            if (course == null)
            {
                throw new NullCourseException($"The course with id: {request.courseId} was not found!");
            }

            //return CourseDto.FromCourse(course);
            _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
            return _mapper.Map<CourseDto>(course);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in course at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }


    }
}
