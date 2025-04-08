using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Courses.Actions;

public record AssignHomework(int CourseId, string Title, string Description, DateTime Deadline) : IRequest<CourseDto>;

public class AssignHomeworkHandler : IRequestHandler<AssignHomework, CourseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignHomeworkHandler> _logger;
    private readonly IMapper _mapper;

    public AssignHomeworkHandler(IUnitOfWork unitOfWork, ILogger<AssignHomeworkHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(AssignHomework request, CancellationToken cancellationToken)
    {
        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.CourseId);

            if (course == null)
                throw new Exception($"Course with ID {request.CourseId} not found.");

            await _unitOfWork.BeginTransactionAsync();

            _unitOfWork.HomeworkRepository.AssignHomeworkToCourse(course, request.Title, request.Description, request.Deadline);
            await _unitOfWork.SaveAsync();

            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation("Homework assigned and student links created.");

            return _mapper.Map<CourseDto>(course);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError($"Error assigning homework: {ex.Message}");
            throw;
        }
    }
}

