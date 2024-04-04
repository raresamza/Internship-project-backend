using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Exceptions.Catalogue;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Delete;

public record DeleteCatalogue(int catalogueId) : IRequest<CatalogueDto>;

public class DeleteCatalogueHandler : IRequestHandler<DeleteCatalogue, CatalogueDto>
{

    private readonly ICatalogueRepository _catalogueRepository;

    public DeleteCatalogueHandler(ICatalogueRepository catalogueRepository)
    {
        _catalogueRepository = catalogueRepository;
    }

    public Task<CatalogueDto> Handle(DeleteCatalogue request, CancellationToken cancellationToken)
    {
        var catalogue=_catalogueRepository.GetById(request.catalogueId);

        if(catalogue == null)
        {
            throw new CatalogueNotFoundException($"The catalogue with id: {request.catalogueId} was not found");
        }

        _catalogueRepository.Delete(catalogue);

        return Task.FromResult(CatalogueDto.FromCatalogue(catalogue));
    }
}
