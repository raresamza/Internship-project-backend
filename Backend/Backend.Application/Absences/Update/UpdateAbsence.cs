using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Update;


public record UpdateAbsence(int absenceId, AbsenceUpdateDto absence) : IRequest<AbsenceDto>;
public class UpdateAbsenceHandler : IRequestHandler<UpdateAbsence, AbsenceDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateAbsenceHandler> _logger;
    public UpdateAbsenceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateAbsenceHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AbsenceDto> Handle(UpdateAbsence request, CancellationToken cancellationToken)
    {
        try
        {
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);

            if (absence == null)
            {
                throw new InvalidAbsenceException($"The absence with id: {request.absenceId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            var newAbs = await _unitOfWork.AbsenceRepository.UpdateAbsence(absence.Id, request.absence);
            await _unitOfWork.CommitTransactionAsync();
            //return AbsenceDto.FromAbsence(newAbs);
            _logger.LogError($"Absence action executed at: {DateTime.Now.TimeOfDay}");

            return _mapper.Map<AbsenceDto>(newAbs);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in absence at: {DateTime.Now.TimeOfDay}");
            Console.Write(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
