using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Actions;
using Backend.Application.Catalogues.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Queries;

public record GetCatalogues() : IRequest<List<CatalogueDto>>;

public class GetCataloguesHandler : IRequestHandler<GetCatalogues, List<CatalogueDto>>
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
    public async Task<List<CatalogueDto>> Handle(GetCatalogues request, CancellationToken cancellationToken)
    {
        var catalogues = await _unitOfWork.CatalogueRepository.GetAll();
        _logger.LogInformation($"Catalogue action executed at: {DateTime.Now.TimeOfDay}");

        return catalogues.Select((catalogue) => _mapper.Map<CatalogueDto>(catalogue)).ToList();
    }
}
