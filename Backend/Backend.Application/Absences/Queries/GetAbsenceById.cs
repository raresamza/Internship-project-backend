using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;


public record GetAbsenceById(int absenceId) : IRequest<AbsenceDto>;
public class GetAbsenceByIdHandler : IRequestHandler<GetAbsenceById, AbsenceDto>
{
    private readonly IAbsenceRepository _absenceRepository;

    public GetAbsenceByIdHandler(IAbsenceRepository absenceRepository)
    {
        _absenceRepository = absenceRepository;
    }

    public Task<AbsenceDto> Handle(GetAbsenceById request, CancellationToken cancellationToken)
    {
        var absence = _absenceRepository.GetById(request.absenceId);
        if (absence == null)
        {
            throw new TeacherNotFoundException($"The absence witrh id: {request.absenceId} was not found!");
        }

        return Task.FromResult(AbsenceDto.FromAbsence(absence));
    }
}
