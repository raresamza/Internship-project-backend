using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Exceptions.Catalogue;
using Backend.Domain.Models;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Delete;

public record DeleteCatalogue(int catalogueId) : IRequest<CatalogueDto>;

public class DeleteCatalogueHandler : IRequestHandler<DeleteCatalogue, CatalogueDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public DeleteCatalogueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork=unitOfWork;
    }

    public async Task<CatalogueDto> Handle(DeleteCatalogue request, CancellationToken cancellationToken)
    {
        try
        {
            Catalogue? catalogue = await _unitOfWork.CatalogueRepository.GetById(request.catalogueId);

            if (catalogue == null)
            {
                throw new CatalogueNotFoundException($"The catalogue with id: {request.catalogueId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.CatalogueRepository.Delete(catalogue);
            await _unitOfWork.CommitTransactionAsync();

            return CatalogueDto.FromCatalogue(catalogue);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }
}
