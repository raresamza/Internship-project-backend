using Backend.Application.Abstractions;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Delete;

public record DeleteSchool(int schoolId) : IRequest<SchoolDto>;
public class DeleteSchoolHandle : IRequestHandler<DeleteSchool, SchoolDto>
{
    private readonly ISchoolRepository _schoolRepository;

    public DeleteSchoolHandle(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public Task<SchoolDto> Handle(DeleteSchool request, CancellationToken cancellationToken)
    {
        var school=_schoolRepository.GetById(request.schoolId);

        if (school == null)
        {
            throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
        }

        _schoolRepository.Delete(school);

        return Task.FromResult(SchoolDto.FromScool(school));
    }
}
