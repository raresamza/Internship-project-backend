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

namespace Backend.Application.Classrooms.Queries;

public record GetClassroomById(int classroomId): IRequest<ClassroomDto>;
public class GetClassroomByIdHandler : IRequestHandler<GetClassroomById, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetClassroomByIdHandler> _logger;
    public GetClassroomByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetClassroomByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<ClassroomDto> IRequestHandler<GetClassroomById, ClassroomDto>.Handle(GetClassroomById request, CancellationToken cancellationToken)
    {

        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
            }
            _logger.LogInformation($"Action in classroom at: {DateTime.Now.TimeOfDay}");

            //return ClassroomDto.FromClassroom(classroom);
            return _mapper.Map<ClassroomDto>(classroom);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in classroom at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }
        
    }
}
