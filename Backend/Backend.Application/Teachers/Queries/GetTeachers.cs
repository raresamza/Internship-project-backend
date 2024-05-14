using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Queries;

public record GetTeachers(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<TeacherDto>>;

public class GetTeachersHandler : IRequestHandler<GetTeachers, PaginatedResult<TeacherDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTeachersHandler> _logger;
    public GetTeachersHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetTeachersHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<PaginatedResult<TeacherDto>> Handle(GetTeachers request, CancellationToken cancellationToken)
    {
        var teachers = await _unitOfWork.TeacherRepository.GetAll();
        var totalCount = teachers.Count;

        var pagedTeachers = teachers
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var teacherDtos = _mapper.Map<List<TeacherDto>>(pagedTeachers);

        _logger.LogInformation($"Retrieved {teacherDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<TeacherDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            teacherDtos
        );
    }
}
