using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Exceptions.Catalogue;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Update;

public record UpdateCatalogue(int catalogueId, CatalogueUpdateDto catalogue) : IRequest<CatalogueDto>;

public class UpdateCatalogueHandler : IRequestHandler<UpdateCatalogue, CatalogueDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAbsenceByDateAndCourseHandler> _logger;
    public UpdateCatalogueHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAbsenceByDateAndCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Catalogue action executed at: {DateTime.Now.TimeOfDay}");

            //return CatalogueDto.FromCatalogue(newCatalogue);
            return _mapper.Map<CatalogueDto>(newCatalogue);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in catalogue at: {DateTime.Now.TimeOfDay}");

            await Console.Out.WriteLineAsync(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }
}
