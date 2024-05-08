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

namespace Backend.Application.Students.Actions;

public record MotivateAbsence(int studentId, int absenceId, int courseId) : IRequest<StudentDto>;

public class MotivateAbsenceHandler : IRequestHandler<MotivateAbsence, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public MotivateAbsenceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(MotivateAbsence request, CancellationToken cancellationToken)
    {

        try
        {

            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);

            if (course == null)
            {
                throw new NullCourseException($"The course with id: {request.courseId} was not found");
            }
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
            }
            if (absence == null)
            {
                throw new NullCourseException($"Course with id: {request.absenceId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.StudentRepository.MotivateAbsence(absence.Date, course, student);
            await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
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