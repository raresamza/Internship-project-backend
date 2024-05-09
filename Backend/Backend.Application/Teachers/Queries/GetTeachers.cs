using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Queries;

public record GetTeachers() : IRequest<List<TeacherDto>>;

public class GetTeachersHandler : IRequestHandler<GetTeachers, List<TeacherDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTeachersHandler> _logger;
    public GetTeachersHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetTeachersHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<List<TeacherDto>> Handle(GetTeachers request, CancellationToken cancellationToken)
    {
        var teachers= await _unitOfWork.TeacherRepository.GetAll();
        //return teachers.Select((teacher) => TeacherDto.FromTeacher(teacher)).ToList();
        _logger.LogInformation($"Action in teacehr at: {DateTime.Now.TimeOfDay}");
        return teachers.Select(teacher => _mapper.Map<TeacherDto>(teacher)).ToList();

    }
}
