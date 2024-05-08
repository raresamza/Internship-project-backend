using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Students.Responses;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;
public record AddStudentToClassroom(int studentId, int classroomId) : IRequest<StudentDto>;
public class AddStudentToClassroomHandler : IRequestHandler<AddStudentToClassroom, StudentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddStudentToClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDto> Handle(AddStudentToClassroom request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"Classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.ClassroomRepository.AddStudent(student, classroom);
            await _unitOfWork.ClassroomRepository.UpdateClassroom(classroom, classroom.ID);
            await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            await _unitOfWork.CommitTransactionAsync();
            return StudentDto.FromStudent(student);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
