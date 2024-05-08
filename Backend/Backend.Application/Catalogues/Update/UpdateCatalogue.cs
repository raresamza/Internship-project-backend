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

    private readonly IUnitOfWork _unitOfWork;

    public UpdateCatalogueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<CatalogueDto> Handle(UpdateCatalogue request, CancellationToken cancellationToken)
    {

        try
        {
            Catalogue? catalogue = await _unitOfWork.CatalogueRepository.GetById(request.catalogueId);
            if (catalogue == null)
            {
                throw new CatalogueNotFoundException($"The catalogue with id: {request.catalogueId} was not found");
            }
            await _unitOfWork.BeginTransactionAsync();
            var newCatalogue = await _unitOfWork.CatalogueRepository.UpdateCatalogue(request.catalogue, catalogue.ID);
            await _unitOfWork.CommitTransactionAsync();
            return CatalogueDto.FromCatalogue(newCatalogue);
        } catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }
}
