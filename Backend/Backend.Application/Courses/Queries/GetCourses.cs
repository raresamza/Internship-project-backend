using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Response;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Queries;

public record GetCourses() : IRequest<List<CourseDto>>;


public class GetCoursesHandler : IRequestHandler<GetCourses, List<CourseDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCoursesHandler> _logger;
    public GetCoursesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCoursesHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<CourseDto>> Handle(GetCourses request, CancellationToken cancellationToken)
    {
        var courses = await _unitOfWork.CourseRepository.GetAll();
        //return courses.Select(course => CourseDto.FromCourse(course)).ToList();
        _logger.LogInformation($"Action in course at: {DateTime.Now.TimeOfDay}");
        return courses.Select(course => _mapper.Map<CourseDto>(course)).ToList();

    }
}
