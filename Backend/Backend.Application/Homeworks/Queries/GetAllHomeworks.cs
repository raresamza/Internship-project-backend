using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Homeworks.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Homeworks.Queries;


public record GetAllHomeworks() : IRequest<List<HomeworkDto>>;
public class GetAllHomeworksHandler : IRequestHandler<GetAllHomeworks, List<HomeworkDto>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllHomeworksHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllHomeworksHandler(IUnitOfWork unitOfWork, ILogger<GetAllHomeworksHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<HomeworkDto>> Handle(GetAllHomeworks request, CancellationToken cancellationToken)
    {
        var homeworks = await _unitOfWork.HomeworkRepository.GetAllHomeworks();

        return _mapper.Map<List<HomeworkDto>>(homeworks);
    }
}
