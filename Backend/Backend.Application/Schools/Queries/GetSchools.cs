using Backend.Application.Abstractions;
using Backend.Application.Schools.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Queries;

public record GetSchools() : IRequest<List<SchoolDto>>;

public class GetSchoolsHandler : IRequestHandler<GetSchools, List<SchoolDto>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetSchoolsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SchoolDto>> Handle(GetSchools request, CancellationToken cancellationToken)
    {
        var schools= await _unitOfWork.SchoolRepository.GetAll();

        return schools.Select(school => SchoolDto.FromScool(school)).ToList();

    }
}
