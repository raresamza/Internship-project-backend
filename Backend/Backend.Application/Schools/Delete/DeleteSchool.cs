using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Update;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Delete;

public record DeleteSchool(int schoolId) : IRequest<SchoolDto>;
public class DeleteSchoolHandle : IRequestHandler<DeleteSchool, SchoolDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteSchoolHandle> _logger;
    public DeleteSchoolHandle(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteSchoolHandle> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SchoolDto> Handle(DeleteSchool request, CancellationToken cancellationToken)
    {
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);

            if (school == null)
            {
                throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.SchoolRepository.Delete(school);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in school at: {DateTime.Now.TimeOfDay}");
            //return SchoolDto.FromScool(school);
            return _mapper.Map<SchoolDto>(school);
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
