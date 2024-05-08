using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;


public record RemoveStudentFromClassroom(int studentId, int classroomId) : IRequest<ClassroomDto>;

public class RemoveStudentFromClassroomHandler : IRequestHandler<RemoveStudentFromClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public RemoveStudentFromClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ClassroomDto> Handle(RemoveStudentFromClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (student == null)
            {
                throw new StudentNotFoundException($"Teacher with id: {request.studentId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.ClassroomRepository.RemoveStudent(student, classroom);
            await _unitOfWork.ClassroomRepository.UpdateClassroom(classroom, classroom.ID);
            await _unitOfWork.CommitTransactionAsync();
            return ClassroomDto.FromClassroom(classroom);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message );
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }
}
