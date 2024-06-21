using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Create;

public record CreateAbsence(int courseId,DateTime date):IRequest<AbsenceDto>;
public class CreateAbsenceHandler : IRequestHandler<CreateAbsence, AbsenceDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAbsenceHandler> _logger;

    public CreateAbsenceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateAbsenceHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AbsenceDto> Handle(CreateAbsence request, CancellationToken cancellationToken)
    {

        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            if (course == null)
            {
                _logger.LogError($"Error in absence at: {DateTime.Now.TimeOfDay}");
                throw new NullCourseException($"Could not found course with id: {request.courseId}");
            }
            var absence = new Absence(request.date) { Course = course, CourseId = request.courseId };

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.AbsenceRepository.CreateAbsence(absence);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Absence action executed at: {DateTime.Now.TimeOfDay}");
            //return AbsenceDto.FromAbsence(absence);
            return _mapper.Map<AbsenceDto>(absence);
        } catch (Exception ex)
        {
            _logger.LogError($"Error when deleting an absence at: {DateTime.Now.TimeOfDay}");
            await _unitOfWork.RollbackTransactionAsync();
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }
        
    }

    //private int GetNextId()
    //{
    //    return _absenceRepository.GetLastId();
    //}
}
