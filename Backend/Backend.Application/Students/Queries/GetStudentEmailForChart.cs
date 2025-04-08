using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using MediatR;

namespace Backend.Application.Students.Queries;


public record GetStudentEmailForChart(int StudentId) : IRequest<string>;

public class GetStudentEmailForChartHandler : IRequestHandler<GetStudentEmailForChart, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentEmailForChartHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(GetStudentEmailForChart request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentRepository.GetById(request.StudentId);
        if (student == null)
            throw new Exception("Student not found");

        return student.ParentEmail;
    }
}
