using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Teachers.Responses;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.TeacherException;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly IMapper _mapper;
    private readonly ILogger<RemoveTeacherFromClassroomHandler> _logger;
    public RemoveTeacherFromClassroomHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RemoveTeacherFromClassroomHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
            //await _unitOfWork.ClassroomRepository.UpdateClassroom(classroom, classroom.ID);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in classroom at: {DateTime.Now.TimeOfDay}");

            //return ClassroomDto.FromClassroom(classroom);
            return _mapper.Map<ClassroomDto>(classroom);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in classroom at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
