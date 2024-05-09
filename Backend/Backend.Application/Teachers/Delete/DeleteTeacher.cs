using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Update;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteTeacherHandler> _logger;
    public DeleteTeacherHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteTeacherHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Action in teacehr at: {DateTime.Now.TimeOfDay}");
            //return TeacherDto.FromTeacher(teacher);
            return _mapper.Map<TeacherDto>(teacher);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in teacehr at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }


    }
}
