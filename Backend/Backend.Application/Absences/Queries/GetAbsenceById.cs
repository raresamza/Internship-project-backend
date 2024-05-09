using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;


public record GetAbsenceById(int absenceId) : IRequest<AbsenceDto>;
public class GetAbsenceByIdHandler : IRequestHandler<GetAbsenceById, AbsenceDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAbsenceByIdHandler> _logger;
    public GetAbsenceByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAbsenceByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AbsenceDto> Handle(GetAbsenceById request, CancellationToken cancellationToken)
    {
        try
        {
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);
            if (absence == null)
            {
                throw new TeacherNotFoundException($"The absence witrh id: {request.absenceId} was not found!");
            }

            //return AbsenceDto.FromAbsence(absence);
            _logger.LogError($"Absence action executed at: {DateTime.Now.TimeOfDay}");
            return _mapper.Map<AbsenceDto>(absence);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in absence at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
