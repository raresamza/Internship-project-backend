using Backend.Application.Abstractions;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Actions;

public record RemoveClassroom(int schoolId, int classroomId) : IRequest<SchoolDto>;


public class RemoveClassroomHandler : IRequestHandler<RemoveClassroom, SchoolDto>
{

    private readonly ISchoolRepository _schoolRepository;
    private readonly IClassroomRepository _classroomRepository;

    public RemoveClassroomHandler(ISchoolRepository schoolRepository, IClassroomRepository classroomRepository)
    {
        _schoolRepository = schoolRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<SchoolDto> Handle(RemoveClassroom request, CancellationToken cancellationToken)
    {
        var school = _schoolRepository.GetById(request.schoolId);
        var classroom = _classroomRepository.GetById(request.classroomId);

        if (school == null)
        {
            throw new SchoolNotFoundException($"The school with id: {request.schoolId} was not found");
        }
        if (classroom == null)
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        _schoolRepository.RemoveClassroom(classroom, school);
        _schoolRepository.UpdateSchool(school, school.ID);

        return Task.FromResult(SchoolDto.FromScool(school));
    }
}
