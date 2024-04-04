using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Exceptions.Catalogue;
using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Update;

public record UpdateCatalogue(int catalogueId,Catalogue catalogue) : IRequest<CatalogueDto>;

public class UpdateCatalogueHandler : IRequestHandler<UpdateCatalogue, CatalogueDto>
{

    private readonly ICatalogueRepository _catalogueRepository;

    public UpdateCatalogueHandler(ICatalogueRepository catalogueRepository)
    {
        _catalogueRepository = catalogueRepository;
    }

    public Task<CatalogueDto> Handle(UpdateCatalogue request, CancellationToken cancellationToken)
    {
        var catalogue=_catalogueRepository.GetById(request.catalogueId);
        if (catalogue == null)
        {
            throw new CatalogueNotFoundException($"The catalogue with id: {request.catalogueId} was not found");
        }
        var newCatalogue = _catalogueRepository.UpdateCatalogue(request.catalogue, catalogue.ID);

        return Task.FromResult(CatalogueDto.FromCatalogue(newCatalogue));
    }
}
