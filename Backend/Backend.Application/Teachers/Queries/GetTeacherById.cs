using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Queries;


public record GetTeacherById(int Id) : IRequest<TeacherDto>;

public class GetTeacherByIdHandler : IRequestHandler<GetTeacherById, TeacherDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetTeacherByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TeacherDto> Handle(GetTeacherById request, CancellationToken cancellationToken)
    {

        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.Id);
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"The teacher witrh id: {request.Id} was not found!");
            }
            return TeacherDto.FromTeacher(teacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
