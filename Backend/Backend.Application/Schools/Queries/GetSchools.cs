using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Update;
using Backend.Application.Schools.Response;
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

public record GetSchools() : IRequest<List<SchoolDto>>;

public class GetSchoolsHandler : IRequestHandler<GetSchools, List<SchoolDto>>
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
    public async Task<List<SchoolDto>> Handle(GetSchools request, CancellationToken cancellationToken)
    {
        var schools= await _unitOfWork.SchoolRepository.GetAll();
        _logger.LogInformation($"Action in school at: {DateTime.Now.TimeOfDay}");
        return schools.Select(school => _mapper.Map<SchoolDto>(school)).ToList();

    }
}
