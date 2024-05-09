using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Actions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<GetCatalogueByIdHandler> _logger;
    public GetCatalogueByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCatalogueByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Catalogue action executed at: {DateTime.Now.TimeOfDay}");

            //return CatalogueDto.FromCatalogue(catalogue);
            return _mapper.Map<CatalogueDto>(catalogue);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in catalogue at: {DateTime.Now.TimeOfDay}");

            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }
        
    }
}
