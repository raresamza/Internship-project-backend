using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;

public record GetAllStudents() : IRequest<List<StudentDto>>;


public class GetAllStudentsHandler : IRequestHandler<GetAllStudents, List<StudentDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllStudentsHandler> _logger;
    public GetAllStudentsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllStudentsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<List<StudentDto>> IRequestHandler<GetAllStudents, List<StudentDto>>.Handle(GetAllStudents request, CancellationToken cancellationToken)
    {
        var students = await _unitOfWork.StudentRepository.GetAll();
        var studentDtos = _mapper.Map<List<StudentDto>>(students);
        return studentDtos;
    }
}
