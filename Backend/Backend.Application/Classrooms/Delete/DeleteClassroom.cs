using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Delete;

public record DeleteClassroom(int classroomId) : IRequest<ClassroomDto>;
public class DeleteClassroomHandler : IRequestHandler<DeleteClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public DeleteClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClassroomDto> Handle(DeleteClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ClassroomRepository.Delete(classroom);
            await _unitOfWork.CommitTransactionAsync();
            return ClassroomDto.FromClassroom(classroom);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
