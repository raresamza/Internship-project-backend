using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Delete;

public record DeleteClassroom(int classroomId) : IRequest<ClassroomDto>;
public class DeleteClassroomHandler : IRequestHandler<DeleteClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteClassroomHandler> _logger;
    public DeleteClassroomHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteClassroomHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClassroomDto> Handle(DeleteClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.ClassroomRepository.Delete(classroom);
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
