using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Schools.Update;
using Backend.Application.Students.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;


public record GetStudents() : IRequest<List<StudentDto>>;

public class GetStudentsHandler : IRequestHandler<GetStudents, List<StudentDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentsHandler> _logger;
    public GetStudentsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetStudentsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<List<StudentDto>> Handle(GetStudents request, CancellationToken cancellationToken)
    {
        var students = await _unitOfWork.StudentRepository.GetAll();
        //return students.Select(student => StudentDto.FromStudent(student)).ToList();
        _logger.LogInformation($"Action in students at: {DateTime.Now.TimeOfDay}");

        return students.Select(student => _mapper.Map<StudentDto>(student)).ToList();
    }

}
