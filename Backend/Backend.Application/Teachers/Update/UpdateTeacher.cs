using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Update;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateTeacherHandler> _logger;
    public UpdateTeacherHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTeacherHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            _logger.LogInformation($"Action in teacehr at: {DateTime.Now.TimeOfDay}");
            //return TeacherDto.FromTeacher(newTeacher);
            return _mapper.Map<TeacherDto>(newTeacher);
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
