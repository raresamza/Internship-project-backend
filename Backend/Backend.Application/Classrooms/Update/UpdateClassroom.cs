using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Update;

public record UpdateClassroom(int classroomId,Classroom classroom): IRequest<ClassroomDto>;
public class UpdateClassroomHandler : IRequestHandler<UpdateClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateClassroomHandler> _logger;
    public UpdateClassroomHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateClassroomHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClassroomDto> Handle(UpdateClassroom request, CancellationToken cancellationToken)
    {

        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }


            await _unitOfWork.BeginTransactionAsync();
            var newClassroom = await _unitOfWork.ClassroomRepository.UpdateClassroom(request.classroom, classroom.ID);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in classroom at: {DateTime.Now.TimeOfDay}");

            //return ClassroomDto.FromClassroom(newClassroom);
            return _mapper.Map<ClassroomDto>(newClassroom);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in classroom at: {DateTime.Now.TimeOfDay}");
            Console.Write(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        
    }
}
