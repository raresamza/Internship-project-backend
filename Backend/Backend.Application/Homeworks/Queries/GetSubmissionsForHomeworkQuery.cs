using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Homeworks.Response;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Homeworks.Queries;

public record GetSubmissionsForHomeworkQuery(int HomeworkId) : IRequest<List<HomeworkSubmissionDto>>;

public class GetSubmissionsForHomeworkQueryHandler : IRequestHandler<GetSubmissionsForHomeworkQuery, List<HomeworkSubmissionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetSubmissionsForHomeworkQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetSubmissionsForHomeworkQueryHandler(IUnitOfWork unitOfWork, ILogger<GetSubmissionsForHomeworkQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<HomeworkSubmissionDto>> Handle(GetSubmissionsForHomeworkQuery request, CancellationToken cancellationToken)
    {
        var submissions = await _unitOfWork.StudentRepository.GetSubmissionsByHomeworkId(request.HomeworkId);

        return _mapper.Map<List<HomeworkSubmissionDto>>(submissions);
    }
}