using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Update;


public record UpdateAbsence(int absenceId, Absence absence) : IRequest<AbsenceDto>;
public class UpdateAbsenceHandler : IRequestHandler<UpdateAbsence, AbsenceDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public UpdateAbsenceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            return AbsenceDto.FromAbsence(newAbs);

        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
