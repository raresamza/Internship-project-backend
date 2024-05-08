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

    private readonly IUnitOfWork _unitOfWork;

    public CreateAbsenceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AbsenceDto> Handle(CreateAbsence request, CancellationToken cancellationToken)
    {

        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            if (course == null)
            {
                throw new NullCourseException($"Could not found course with id: {request.courseId}");
            }
            var absence = new Absence() { Course = course, CourseId = request.courseId };

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.AbsenceRepository.CreateAbsence(absence);
            await _unitOfWork.CommitTransactionAsync();

            return AbsenceDto.FromAbsence(absence);
        } catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }
        
    }

    //private int GetNextId()
    //{
    //    return _absenceRepository.GetLastId();
    //}
}
