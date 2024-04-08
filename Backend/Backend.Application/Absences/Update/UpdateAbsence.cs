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


public record UpdateAbsence(int absenceId,Absence absence) : IRequest<AbsenceDto>;
public class UpdateAbsenceHandler : IRequestHandler<UpdateAbsence, AbsenceDto>
{

    private readonly IAbsenceRepository _absenceRepository;

    public UpdateAbsenceHandler(IAbsenceRepository absenceRepository)
    {
        _absenceRepository = absenceRepository;
    }

    public Task<AbsenceDto> Handle(UpdateAbsence request, CancellationToken cancellationToken)
    {
        var absence=_absenceRepository.GetById(request.absenceId);

        if(absence == null)
        {
            throw new InvalidAbsenceException($"The absence with id: {request.absenceId} was not found");
        }

        var newAbs=_absenceRepository.UpdateAbsence(absence.Id, request.absence);

        return Task.FromResult(AbsenceDto.FromAbsence(newAbs));

    }
}
