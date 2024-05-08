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

    private readonly IUnitOfWork _unitOfWork;

    public RemoveClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SchoolDto> Handle(RemoveClassroom request, CancellationToken cancellationToken)
    {

        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (school == null)
            {
                throw new SchoolNotFoundException($"The school with id: {request.schoolId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.SchoolRepository.RemoveClassroom(classroom, school);
            await _unitOfWork.SchoolRepository.Update(school.ID, school);
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
