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
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSchoolHandle(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SchoolDto> Handle(DeleteSchool request, CancellationToken cancellationToken)
    {
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);

            if (school == null)
            {
                throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.SchoolRepository.Delete(school);
            await _unitOfWork.CommitTransactionAsync();
            return SchoolDto.FromScool(school);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
