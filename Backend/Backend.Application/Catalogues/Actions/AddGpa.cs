using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Actions;

public record AddGpa(int studentId, int courseId) : IRequest<StudentDto>;
public class AddGpaHandler : IRequestHandler<AddGpa, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public AddGpaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    async Task<StudentDto> IRequestHandler<AddGpa, StudentDto>.Handle(AddGpa request, CancellationToken cancellationToken)
    {

        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            if (course == null)
            {
                throw new NullCourseException($"Course with id: {request.courseId} was not found");
            }
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
            }


            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.CatalogueRepository.AddGpa(course, student);
            await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            await _unitOfWork.CommitTransactionAsync();
            return StudentDto.FromStudent(student);

        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
