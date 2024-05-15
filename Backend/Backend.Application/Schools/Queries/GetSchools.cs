using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Update;
using Backend.Application.Schools.Response;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Queries;

public record GetSchools(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<SchoolDto>>;

public class GetSchoolsHandler : IRequestHandler<GetSchools, PaginatedResult<SchoolDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSchoolsHandler> _logger;
    public GetSchoolsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetSchoolsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<PaginatedResult<SchoolDto>> Handle(GetSchools request, CancellationToken cancellationToken)
    {
        var schools = await _unitOfWork.StudentRepository.GetAll();
        var totalCount = schools.Count;

        var pagedSchools = schools
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var schoolDtos = _mapper.Map<List<SchoolDto>>(pagedSchools);

        _logger.LogInformation($"Retrieved {schoolDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<SchoolDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            schoolDtos
        );

    }
}
