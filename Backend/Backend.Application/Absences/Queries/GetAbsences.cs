using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<GetAbsencesHandler> _logger;
    public GetAbsencesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAbsencesHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<List<AbsenceDto>> Handle(GetAbsences request, CancellationToken cancellationToken)
    {
        var absences = await _unitOfWork.AbsenceRepository.GetAll();

        //return absences.Select((absence) => AbsenceDto.FromAbsence(absence)).ToList();
        _logger.LogError($"Absence action executed at: {DateTime.Now.TimeOfDay}");
        return absences.Select(absence => _mapper.Map<AbsenceDto>(absence)).ToList();

    }
}
