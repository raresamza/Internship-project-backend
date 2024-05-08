using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;


public record GetAbsenceById(int absenceId) : IRequest<AbsenceDto>;
public class GetAbsenceByIdHandler : IRequestHandler<GetAbsenceById, AbsenceDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAbsenceByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AbsenceDto> Handle(GetAbsenceById request, CancellationToken cancellationToken)
    {
        try
        {
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);
            if (absence == null)
            {
                throw new TeacherNotFoundException($"The absence witrh id: {request.absenceId} was not found!");
            }

            return AbsenceDto.FromAbsence(absence);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
