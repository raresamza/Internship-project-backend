using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Update;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Update;

public record UpdateSchool(int schoolId, UpdateSchoolDto school) : IRequest<SchoolDto>;

public class UpdateSchoolHandler : IRequestHandler<UpdateSchool, SchoolDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSchoolHandler> _logger;
    public UpdateSchoolHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateSchoolHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SchoolDto> Handle(UpdateSchool request, CancellationToken cancellationToken)
    {
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);

            if (school == null)
            {
                throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            var newSchool = await _unitOfWork.SchoolRepository.Update(school.ID, school);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in school at: {DateTime.Now.TimeOfDay}");
            //return SchoolDto.FromScool(newSchool);
            return _mapper.Map<SchoolDto>(newSchool);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in school at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
