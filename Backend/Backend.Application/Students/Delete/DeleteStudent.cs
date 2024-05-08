using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Delete;

public record DeleteStudent(int studentId) : IRequest<StudentDto>;
public class DeleteStudentHandler : IRequestHandler<DeleteStudent, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public DeleteStudentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(DeleteStudent request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.StudentRepository.Delete(student);
            await _unitOfWork.CommitTransactionAsync();
            return StudentDto.FromStudent(student);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
