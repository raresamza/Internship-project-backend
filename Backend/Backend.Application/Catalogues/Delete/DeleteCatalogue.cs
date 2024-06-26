﻿using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Actions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Exceptions.Catalogue;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCatalogueHandler> _logger;
    public DeleteCatalogueHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteCatalogueHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Catalogue action executed at: {DateTime.Now.TimeOfDay}");

            //return CatalogueDto.FromCatalogue(catalogue);
            return _mapper.Map<CatalogueDto>(catalogue);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in catalogue at: {DateTime.Now.TimeOfDay}");

            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }
}
