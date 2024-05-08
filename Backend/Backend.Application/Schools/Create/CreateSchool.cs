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

public record CreateSchool(string name) : IRequest<SchoolDto>;
public class CreateSchoolHandler : IRequestHandler<CreateSchool, SchoolDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public CreateSchoolHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SchoolDto> Handle(CreateSchool request, CancellationToken cancellationToken)
    {
        try
        {
            var school = new School() { Name = request.name };
            await _unitOfWork.BeginTransactionAsync();
            var newSchool = await _unitOfWork.SchoolRepository.Create(school);
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
