using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Response;
using Backend.Application.Schools.Queries;
using Backend.Application.Schools.Response;
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

public record GetCourses(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<CourseDto>>;

public class GetCoursesHandler : IRequestHandler<GetCourses, PaginatedResult<CourseDto>>
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

    public async Task<PaginatedResult<CourseDto>> Handle(GetCourses request, CancellationToken cancellationToken)
    {
        var courses = await _unitOfWork.CourseRepository.GetAll();
        var totalCount = courses.Count;

        var pagedCoruses = courses
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var courseDtos = _mapper.Map<List<CourseDto>>(pagedCoruses);

        _logger.LogInformation($"Retrieved {courseDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<CourseDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            courseDtos
        );


    }
}
