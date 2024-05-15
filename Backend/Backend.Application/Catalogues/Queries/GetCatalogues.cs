using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Actions;
using Backend.Application.Catalogues.Response;
using Backend.Application.Classrooms.Queries;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Queries;

public record GetCatalogues(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<CatalogueDto>>;

public class GetCataloguesHandler : IRequestHandler<GetCatalogues, PaginatedResult<CatalogueDto>>
{ 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCataloguesHandler> _logger;
    public GetCataloguesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCataloguesHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<PaginatedResult<CatalogueDto>> Handle(GetCatalogues request, CancellationToken cancellationToken)
    {
        var catalogues = await _unitOfWork.StudentRepository.GetAll();
        var totalCount = catalogues.Count;

        var pagedCatalogues = catalogues
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var catalogueDtos = _mapper.Map<List<CatalogueDto>>(pagedCatalogues);

        _logger.LogInformation($"Retrieved {catalogueDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<CatalogueDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            catalogueDtos
        );
    }
}
