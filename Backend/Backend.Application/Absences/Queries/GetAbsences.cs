using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;


public record GetAbsences() : IRequest<List<AbsenceDto>>;

public class GetAbsencesHandler : IRequestHandler<GetAbsences, List<AbsenceDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    public GetAbsencesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<AbsenceDto>> Handle(GetAbsences request, CancellationToken cancellationToken)
    {
        var absences = await _unitOfWork.AbsenceRepository.GetAll();

        return absences.Select((absence) => AbsenceDto.FromAbsence(absence)).ToList();
    }
}
