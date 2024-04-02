using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Queries;


public record GetCatalogueById(int catalogueId) : IRequest<CatalogueDto>;

public class GetCatalogueByIdHandler : IRequestHandler<GetCatalogueById, CatalogueDto>
{

    private readonly ICatalogueRepository _catalogueRepository;

    public GetCatalogueByIdHandler(ICatalogueRepository catalogueRepository)
    {
        _catalogueRepository = catalogueRepository;
    }

    public Task<CatalogueDto> Handle(GetCatalogueById request, CancellationToken cancellationToken)
    {
        var catalogue=_catalogueRepository.GetById(request.catalogueId);
        if(catalogue == null)
        {
            throw new ArgumentNullException($"The catalogue with id:{request.catalogueId} was not found");
        }

        return Task.FromResult(CatalogueDto.FromCatalogue(catalogue));
    }
}
