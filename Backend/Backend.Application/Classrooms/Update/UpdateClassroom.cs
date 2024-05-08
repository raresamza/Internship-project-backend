using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Update;

public record UpdateClassroom(int classroomId,Classroom classroom): IRequest<ClassroomDto>;
public class UpdateClassroomHandler : IRequestHandler<UpdateClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public UpdateClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClassroomDto> Handle(UpdateClassroom request, CancellationToken cancellationToken)
    {

        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }


            await _unitOfWork.BeginTransactionAsync();
            var newClassroom = await _unitOfWork.ClassroomRepository.UpdateClassroom(request.classroom, classroom.ID);
            await _unitOfWork.CommitTransactionAsync();
            return ClassroomDto.FromClassroom(newClassroom);
        } catch (Exception ex)
        {
            Console.Write(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        
    }
}
