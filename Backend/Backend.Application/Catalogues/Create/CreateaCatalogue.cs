using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Actions;
using Backend.Application.Catalogues.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Create;

public record CreateCatalogue(int classroomId) : IRequest<CatalogueDto>;
public class CreateaCatalogueHandler : IRequestHandler<CreateCatalogue, CatalogueDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateaCatalogueHandler> _logger;
    public CreateaCatalogueHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateaCatalogueHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CatalogueDto> Handle(CreateCatalogue request, CancellationToken cancellationToken)
    {
        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom wiht id: {request.classroomId} was not found");
            }

            var catalogue = new Catalogue() { Classroom = classroom };

            await _unitOfWork.BeginTransactionAsync();
            var newCatalogue = await _unitOfWork.CatalogueRepository.Create(catalogue);
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation($"Catalogue action executed at: {DateTime.Now.TimeOfDay}");

            //return CatalogueDto.FromCatalogue(newCatalogue);
            return _mapper.Map<CatalogueDto>(catalogue);

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
