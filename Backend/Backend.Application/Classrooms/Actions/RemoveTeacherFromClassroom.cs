using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record RemoveTeacherFromClassroom(int teacherId, int classroomId) : IRequest<ClassroomDto>;
public class RemoveTeacherFromClassroomHandler : IRequestHandler<RemoveTeacherFromClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public RemoveTeacherFromClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ClassroomDto> Handle(RemoveTeacherFromClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.teacherId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (teacher == null)
            {
                throw new TeacherNotFoundException($"Teacher with id: {request.teacherId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.ClassroomRepository.RemoveTeacher(teacher, classroom);
            await _unitOfWork.ClassroomRepository.UpdateClassroom(classroom, classroom.ID);
            await _unitOfWork.CommitTransactionAsync();
            return ClassroomDto.FromClassroom(classroom);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
