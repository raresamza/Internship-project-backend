using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Queries;

public record GetStudentById(int studentId) : IRequest<StudentDto>;



public class GetStudentByIdHandler : IRequestHandler<GetStudentById, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(GetStudentById request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
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
