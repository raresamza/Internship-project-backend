using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Create;

public record CreateAbsence(int courseId):IRequest<AbsenceDto>;
public class CreateAbsenceHandler : IRequestHandler<CreateAbsence, AbsenceDto>
{

    private readonly IAbsenceRepository _absenceRepository;
    private readonly ICourseRepository _courseRepository;

    public CreateAbsenceHandler(IAbsenceRepository absenceReposutory, ICourseRepository courseRepository)
    {
        _absenceRepository = absenceReposutory;
        _courseRepository = courseRepository;
    }

    public Task<AbsenceDto> Handle(CreateAbsence request, CancellationToken cancellationToken)
    {
        var course = _courseRepository.GetById(request.courseId);
        if (course == null)
        {
            throw new NullCourseException($"Could not found course with id: {request.courseId}");
        }
        var absence = new Absence() { Course = course,CourseId = request.courseId};

        _absenceRepository.CreateAbsence(absence);

        return Task.FromResult(AbsenceDto.FromAbsence(absence));
    }

    //private int GetNextId()
    //{
    //    return _absenceRepository.GetLastId();
    //}
}
