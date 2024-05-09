using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
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

public record AddTeacherToClassroom(int teacherId, int classroomId) : IRequest<ClassroomDto>;

public class AddTeacherToClassroomHandler : IRequestHandler<AddTeacherToClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddTeacherToClassroomHandler> _logger;
    public AddTeacherToClassroomHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddTeacherToClassroomHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
