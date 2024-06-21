using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Queries;

public record GetCorusesBySubject(Subject Subject) : IRequest<List<CourseDto>>;


public class GetCorusesBySubjectHandler : IRequestHandler<GetCorusesBySubject, List<CourseDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCorusesBySubjectHandler> _logger;
    public GetCorusesBySubjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCorusesBySubjectHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<CourseDto>> Handle(GetCorusesBySubject request, CancellationToken cancellationToken)
    {
        var courses = await _unitOfWork.CourseRepository.GetAll();
        var correctCourses = courses.Where(course => course.Subject == request.Subject).ToList();
        var courseDtos = _mapper.Map<List<CourseDto>>(correctCourses);


        return courseDtos;
    }
}
