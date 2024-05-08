using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Exceptions.AbsenceException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Delete;

public record DeleteAbsence(int absenceId) : IRequest<AbsenceDto>;
public class DeleteAbsenceHandler : IRequestHandler<DeleteAbsence, AbsenceDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public DeleteAbsenceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AbsenceDto> Handle(DeleteAbsence request, CancellationToken cancellationToken)
    {

        try
        {
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);
            if (absence == null)
            {
                throw new InvalidAbsenceException($"The absence with id: {request.absenceId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.AbsenceRepository.DeleteAbsence(absence);
            await _unitOfWork.CommitTransactionAsync();

            return AbsenceDto.FromAbsence(absence);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
