using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Models;
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

    private readonly IUnitOfWork _unitOfWork;

    public GetCatalogueByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CatalogueDto> Handle(GetCatalogueById request, CancellationToken cancellationToken)
    {
        try
        {
            Catalogue? catalogue = await _unitOfWork.CatalogueRepository.GetById(request.catalogueId);
            if (catalogue == null)
            {
                throw new ArgumentNullException($"The catalogue with id:{request.catalogueId} was not found");
            }

            return CatalogueDto.FromCatalogue(catalogue);
        } catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }
        
    }
}
