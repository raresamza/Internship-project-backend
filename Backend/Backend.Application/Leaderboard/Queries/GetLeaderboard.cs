using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Leaderboard.Responses;
using MediatR;

namespace Backend.Application.Leaderboard.Queries;

public record GetLeaderboard() : IRequest<List<ClassStudentLeaderboardDto>>;

public class GetLeaderboardHandler : IRequestHandler<GetLeaderboard, List<ClassStudentLeaderboardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetLeaderboardHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ClassStudentLeaderboardDto>> Handle(GetLeaderboard request, CancellationToken cancellationToken)
    {
        var leaderboardData = await _unitOfWork.StudentRepository.GetLeaderboardData();
        return leaderboardData.Select(_mapper.Map<ClassStudentLeaderboardDto>).ToList();
    }
}
