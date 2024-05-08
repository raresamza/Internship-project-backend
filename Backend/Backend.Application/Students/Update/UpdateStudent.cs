using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Update;

public record UpdateStudent(int studentId, Domain.Models.Student student) : IRequest<StudentDto>;
public class UpdateStudentHandler : IRequestHandler<UpdateStudent, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStudentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(UpdateStudent request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
            }

            var newStudent = await _unitOfWork.StudentRepository.UpdateStudent(request.student, student.ID);

            return StudentDto.FromStudent(newStudent);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
