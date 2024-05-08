using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using MediatR;
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
    public GetCataloguesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<CatalogueDto>> Handle(GetCatalogues request, CancellationToken cancellationToken)
    {
        var catalogues = await _unitOfWork.CatalogueRepository.GetAll();
        return catalogues.Select((catalogue) => CatalogueDto.FromCatalogue(catalogue)).ToList();
    }
}
