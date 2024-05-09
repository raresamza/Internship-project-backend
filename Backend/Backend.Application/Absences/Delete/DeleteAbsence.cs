using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Exceptions.AbsenceException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Delete;

public record DeleteAbsence(int absenceId) : IRequest<AbsenceDto>;
public class DeleteAbsenceHandler : IRequestHandler<DeleteAbsence, AbsenceDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteAbsenceHandler> _logger;

    public DeleteAbsenceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteAbsenceHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AbsenceDto> Handle(DeleteAbsence request, CancellationToken cancellationToken)
    {

        try
        {
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);
            if (absence == null)
            {
                _logger.LogError($"Error in absence at: {DateTime.Now.TimeOfDay}");
                throw new InvalidAbsenceException($"The absence with id: {request.absenceId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.AbsenceRepository.DeleteAbsence(absence);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogError($"Absence action executed at: {DateTime.Now.TimeOfDay}");

            //return AbsenceDto.FromAbsence(absence);
            return _mapper.Map<AbsenceDto>(absence);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            _logger.LogError($"Error when deleting an absence at: {DateTime.Now.TimeOfDay}");
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
