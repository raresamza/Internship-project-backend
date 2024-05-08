using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Students.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;


public record GetStudents() : IRequest<List<StudentDto>>;

public class GetStudentsHandler : IRequestHandler<GetStudents, List<StudentDto>>
{

    public readonly IUnitOfWork _unitOfWork;

    public GetStudentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<StudentDto>> Handle(GetStudents request, CancellationToken cancellationToken)
    {
        var students = await _unitOfWork.StudentRepository.GetAll();
        return students.Select(student => StudentDto.FromStudent(student)).ToList();
    }

}
