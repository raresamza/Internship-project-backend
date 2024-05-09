using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Queries;


public record GetTeacherById(int Id) : IRequest<TeacherDto>;

public class GetTeacherByIdHandler : IRequestHandler<GetTeacherById, TeacherDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTeacherByIdHandler> _logger;
    public GetTeacherByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetTeacherByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TeacherDto> Handle(GetTeacherById request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.Id);
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"The teacher witrh id: {request.Id} was not found!");
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
