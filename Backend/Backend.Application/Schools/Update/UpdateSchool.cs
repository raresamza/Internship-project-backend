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

    private IUnitOfWork _unitOfWork;

    public UpdateSchoolHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SchoolDto> Handle(UpdateSchool request, CancellationToken cancellationToken)
    {
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);

            if (school == null)
            {
                throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            var newSchool = await _unitOfWork.SchoolRepository.Update(school.ID, school);
            await _unitOfWork.CommitTransactionAsync();
            return SchoolDto.FromScool(newSchool);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
