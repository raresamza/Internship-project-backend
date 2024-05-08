using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Queries;


public record GetClassrooms() : IRequest<List<ClassroomDto>>;

public class GetClassroomsHandler : IRequestHandler<GetClassrooms, List<ClassroomDto>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetClassroomsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ClassroomDto>> Handle(GetClassrooms request, CancellationToken cancellationToken)
    {
        var classrooms = await _unitOfWork.ClassroomRepository.GetAll();

        return classrooms.Select((classroom) => ClassroomDto.FromClassroom(classroom)).ToList();
    }
}
