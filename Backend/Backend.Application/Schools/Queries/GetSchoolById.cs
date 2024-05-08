using Backend.Application.Abstractions;
using Backend.Application.Schools.Response;
using Backend.Domain.Exceptions.SchoolException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Queries;

public record GetSchoolById(int schoolId) : IRequest<SchoolDto>;
public class GetSchoolByIdHandler : IRequestHandler<GetSchoolById, SchoolDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetSchoolByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SchoolDto> Handle(GetSchoolById request, CancellationToken cancellationToken)
    {
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);
            if (school == null)
            {
                throw new SchoolNotFoundException($"School with id: {request.schoolId} was not found");
            }

            return SchoolDto.FromScool(school);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
