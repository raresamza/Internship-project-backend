using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Exceptions.AbsenceException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Delete;

public record DeleteAbsence(int absenceId): IRequest<AbsenceDto>;
public class DeleteAbsenceHandler : IRequestHandler<DeleteAbsence, AbsenceDto>
{

    private readonly IAbsenceRepository _absenceRepository;

    public DeleteAbsenceHandler(IAbsenceRepository absenceRepository)
    {
        _absenceRepository = absenceRepository;
    }

    public Task<AbsenceDto> Handle(DeleteAbsence request, CancellationToken cancellationToken)
    {
        var absence=_absenceRepository.GetById(request.absenceId);
        if (absence == null)
        {
            throw new InvalidAbsenceException($"The absence with id: {request.absenceId} was not found");
        }

        _absenceRepository.DeleteAbsence(absence);

        return Task.FromResult(AbsenceDto.FromAbsence(absence));
    }
}
