using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record AddTeacherToClassroom(int teacherId, int classroomId) : IRequest<ClassroomDto>;

public class AddTeacherToClassroomHandler : IRequestHandler<AddTeacherToClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public AddTeacherToClassroomHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ClassroomDto> Handle(AddTeacherToClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.teacherId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (teacher == null)
            {
                throw new TeacherNotFoundException($"The teacher with id: {request.teacherId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }


            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.ClassroomRepository.AddTeacher(teacher, classroom);
            //_classroomRepository.UpdateClassroom(classroom,classroom.ID);
            await _unitOfWork.TeacherRepository.UpdateTeacher(teacher, teacher.ID);
            await _unitOfWork.CommitTransactionAsync();
            return ClassroomDto.FromClassroom(classroom);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }
}
