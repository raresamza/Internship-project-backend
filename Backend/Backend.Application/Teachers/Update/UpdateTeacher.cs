using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Update;

public record UpdateTeacher(int teacherId, Teacher teacher) : IRequest<TeacherDto>;
public class UpdateTeacherHandler : IRequestHandler<UpdateTeacher, TeacherDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public UpdateTeacherHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TeacherDto> Handle(UpdateTeacher request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.teacherId);

            if (teacher == null)
            {
                throw new TeacherNotFoundException($"The teacher with id: {request.teacher} was not found");
            }
            await _unitOfWork.BeginTransactionAsync();
            var newTeacher = await _unitOfWork.TeacherRepository.UpdateTeacher(request.teacher, teacher.ID);
            await _unitOfWork.CommitTransactionAsync();
            return TeacherDto.FromTeacher(newTeacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
