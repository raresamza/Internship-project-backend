using Backend.Application.Schools.Response;
using MediatR;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;

namespace Backend.Application.Schools.Create;

public record CreateSchool(string name): IRequest<SchoolDto>;
public class CreateSchoolHandler : IRequestHandler<CreateSchool, SchoolDto>
{

    private readonly ISchoolRepository _schoolRepository;

    public CreateSchoolHandler(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public Task<SchoolDto> Handle(CreateSchool request, CancellationToken cancellationToken)
    {
        var school=new School() { Name=request.name,ID=GetNextId()};
        var newSchool = _schoolRepository.Create(school);

        return Task.FromResult(SchoolDto.FromScool(newSchool));
    }

    private int GetNextId()
    {
        return _schoolRepository.GetLastId();
    }
}
