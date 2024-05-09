using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Queries;


public record GetClassrooms() : IRequest<List<ClassroomDto>>;

public class GetClassroomsHandler : IRequestHandler<GetClassrooms, List<ClassroomDto>>
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

    public async Task<List<ClassroomDto>> Handle(GetClassrooms request, CancellationToken cancellationToken)
    {
        var classrooms = await _unitOfWork.ClassroomRepository.GetAll();
        _logger.LogInformation($"Action in classroom at: {DateTime.Now.TimeOfDay}");

        return classrooms.Select((classroom) => _mapper.Map<ClassroomDto>(classroom)).ToList();
    }
}
