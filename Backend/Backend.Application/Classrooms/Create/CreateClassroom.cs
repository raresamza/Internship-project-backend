using Backend.Application.Classrooms.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Exceptions.SchoolException;
using AutoMapper;
using Backend.Application.Absences.Queries;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Classrooms.Create;


public record CreateClassroom(string name, int schoolId) : IRequest<ClassroomDto>;
public class CreateaClassroomHandler : IRequestHandler<CreateClassroom, ClassroomDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateaClassroomHandler> _logger;
    public CreateaClassroomHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateaClassroomHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClassroomDto> Handle(CreateClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(request.schoolId);
            if (school == null)
            {
                throw new SchoolNotFoundException($"School with id {request.schoolId} was not found");
            }
            var classroom = new Classroom() { Name = request.name, SchoolId = request.schoolId, School = school };

            await _unitOfWork.BeginTransactionAsync();
            var createdClassroom = await _unitOfWork.ClassroomRepository.Create(classroom);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in classroom at: {DateTime.Now.TimeOfDay}");

            //return ClassroomDto.FromClassroom(createdClassroom);
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
