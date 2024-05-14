using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Backend.Application.Students.Queries;


public record GetStudents(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<StudentDto>>;

public class GetStudentsHandler : IRequestHandler<GetStudents, PaginatedResult<StudentDto>>
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

    public async Task<PaginatedResult<StudentDto>> Handle(GetStudents request, CancellationToken cancellationToken)
    {
        var students = await _unitOfWork.StudentRepository.GetAll();
        var totalCount = students.Count;

        var pagedStudents = students
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var studentDtos = _mapper.Map<List<StudentDto>>(pagedStudents);

        _logger.LogInformation($"Retrieved {studentDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<StudentDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            studentDtos
        );
    }
}
