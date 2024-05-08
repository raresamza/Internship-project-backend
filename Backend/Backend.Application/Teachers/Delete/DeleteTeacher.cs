using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Delete;

public record DeleteTeacher(int teacherId) : IRequest<TeacherDto>;
public class DeleteTeacherHandler : IRequestHandler<DeleteTeacher, TeacherDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public DeleteTeacherHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TeacherDto> Handle(DeleteTeacher request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.teacherId);
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"The teacher with id: {request.teacherId} was not found");
            }
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.TeacherRepository.Delete(teacher);
            await _unitOfWork.CommitTransactionAsync();
            return TeacherDto.FromTeacher(teacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }


    }
}
