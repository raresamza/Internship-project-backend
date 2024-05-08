using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Queries;

public record GetTeachers() : IRequest<List<TeacherDto>>;

public class GetTeachersHandler : IRequestHandler<GetTeachers, List<TeacherDto>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetTeachersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<TeacherDto>> Handle(GetTeachers request, CancellationToken cancellationToken)
    {
        var teachers= await _unitOfWork.TeacherRepository.GetAll();
        return teachers.Select((teacher) => TeacherDto.FromTeacher(teacher)).ToList();
    }
}
