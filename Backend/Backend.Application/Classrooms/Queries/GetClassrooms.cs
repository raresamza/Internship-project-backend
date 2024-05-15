using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Queries;


public record GetClassrooms(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<ClassroomDto>>;

public class GetClassroomsHandler : IRequestHandler<GetClassrooms, PaginatedResult<ClassroomDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetClassroomsHandler> _logger;
    public GetClassroomsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetClassroomsHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResult<ClassroomDto>> Handle(GetClassrooms request, CancellationToken cancellationToken)
    {
        var classrooms = await _unitOfWork.ClassroomRepository.GetAll();
        var totalCount = classrooms.Count;

        var pagedClassrooms = classrooms
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var classroomDtos = _mapper.Map<List<ClassroomDto>>(pagedClassrooms);

        _logger.LogInformation($"Retrieved {classroomDtos.Count} students at: {DateTime.Now.TimeOfDay}");

        return new PaginatedResult<ClassroomDto>(
            request.PageNumber,
            request.PageSize,
            totalCount,
            classroomDtos
        );

    }
}
