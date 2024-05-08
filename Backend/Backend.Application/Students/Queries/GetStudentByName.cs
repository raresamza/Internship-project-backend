using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;

public record GetStudentByName(string studentName) : IRequest<StudentDto>;
public class GetStudentByNameHandler : IRequestHandler<GetStudentByName, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetStudentByNameHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(GetStudentByName request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetByName(request.studentName);
            if (student == null)
            {
                throw new ApplicationException("Student not found");
            }

            return StudentDto.FromStudent(student);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
