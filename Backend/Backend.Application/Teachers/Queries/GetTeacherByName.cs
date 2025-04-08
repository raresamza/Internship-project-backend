using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Teachers.Queries;

public record GetTeacherByName(string name) : IRequest<TeacherDto>;

public class GetTeacherByNameHandler : IRequestHandler<GetTeacherByName, TeacherDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTeacherByNameHandler> _logger;
    public GetTeacherByNameHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetTeacherByNameHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TeacherDto> Handle(GetTeacherByName request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetByName(request.name);
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"The teacher with name: {request.name} was not found!");
            }
            //return TeacherDto.FromTeacher(teacher);
            _logger.LogInformation($"Action in teacehr at: {DateTime.Now.TimeOfDay}");
            return _mapper.Map<TeacherDto>(teacher);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in teacehr at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}