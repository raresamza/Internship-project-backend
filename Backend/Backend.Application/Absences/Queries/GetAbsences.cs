using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Queries;
using Backend.Application.Catalogues.Response;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;


public record GetAbsences(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<AbsenceDto>>;

public class GetAbsencesHandler : IRequestHandler<GetAbsences, PaginatedResult<AbsenceDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAbsencesHandler> _logger;
    public GetAbsencesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAbsencesHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<PaginatedResult<AbsenceDto>> Handle(GetAbsences request, CancellationToken cancellationToken)
    {
        var absences = await _unitOfWork.AbsenceRepository.GetAll();
        var totalCount = absences.Count;

        var pagedAbsences = absences
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var absenceDtos = _mapper.Map<List<AbsenceDto>>(pagedAbsences);

        _logger.LogInformation($"Retrieved {absenceDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<AbsenceDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            absenceDtos
        );

    }
}
