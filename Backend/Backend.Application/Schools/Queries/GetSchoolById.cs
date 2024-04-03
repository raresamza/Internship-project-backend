using Backend.Application.Abstractions;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Queries;

public record GetSchoolById(int schoolId):IRequest<SchoolDto>;
public class GetSchoolByIdHandler : IRequestHandler<GetSchoolById, SchoolDto>
{

    private readonly ISchoolRepository _schoolRepository;

    public GetSchoolByIdHandler(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public Task<SchoolDto> Handle(GetSchoolById request, CancellationToken cancellationToken)
    {
        var school= _schoolRepository.GetById(request.schoolId);
        if(school == null)
        {
            throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
        }

        return Task.FromResult(SchoolDto.FromScool(school));
    }
}
