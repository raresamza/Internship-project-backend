using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Queries;

public record GetClassroomById(int classroomId): IRequest<ClassroomDto>;
public class GetClassroomByIdHandler : IRequestHandler<GetClassroomById, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetClassroomByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    async Task<ClassroomDto> IRequestHandler<GetClassroomById, ClassroomDto>.Handle(GetClassroomById request, CancellationToken cancellationToken)
    {

        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }

            return ClassroomDto.FromClassroom(classroom);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        
    }
}
