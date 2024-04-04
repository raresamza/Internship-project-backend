using Backend.Application.Abstractions;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Update;

public record UpdateSchool(int schoolId, School school) : IRequest<SchoolDto>;

public class UpdateSchoolHandler : IRequestHandler<UpdateSchool, SchoolDto>
{

    private ISchoolRepository _schoolRepository;

    public UpdateSchoolHandler(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public Task<SchoolDto> Handle(UpdateSchool request, CancellationToken cancellationToken)
    {

        var school = _schoolRepository.GetById(request.schoolId);

        if (school == null)
        {
            throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
        }

        var newSchool = _schoolRepository.Update(school.ID,school);

        return Task.FromResult(SchoolDto.FromScool(newSchool));
    }
}
