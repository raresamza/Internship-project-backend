using Backend.Application.Schools.Response;
using MediatR;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using AutoMapper;
using Backend.Application.Courses.Update;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Schools.Create;

public record CreateSchool(string name) : IRequest<SchoolDto>;
public class CreateSchoolHandler : IRequestHandler<CreateSchool, SchoolDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSchoolHandler> _logger;
    public CreateSchoolHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateSchoolHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SchoolDto> Handle(CreateSchool request, CancellationToken cancellationToken)
    {
        try
        {
            var school = new UpdateSchoolDto() { Name = request.name };
            await _unitOfWork.BeginTransactionAsync();
            var newSchool = await _unitOfWork.SchoolRepository.Create(school);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in school at: {DateTime.Now.TimeOfDay}");
            //return SchoolDto.FromScool(newSchool);
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
