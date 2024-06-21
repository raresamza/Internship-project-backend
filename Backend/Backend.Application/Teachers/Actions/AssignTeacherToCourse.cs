using Backend.Application.Abstractions;
using Backend.Application.Teachers.Responses;
using MediatR;
using System;
using Backend.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.TeacherException;
using System.Runtime.InteropServices;
using AutoMapper;
using Backend.Application.Students.Update;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Teachers.Actions;

//public void AssignToCourse(int courseID, int teacherID);
public record AssignTeacherToCourse(int courseId, int teacherId) : IRequest<TeacherDto>;

public class AssignTeacherToCourseHandler : IRequestHandler<AssignTeacherToCourse, TeacherDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AssignTeacherToCourseHandler> _logger;
    public AssignTeacherToCourseHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AssignTeacherToCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TeacherDto> Handle(AssignTeacherToCourse request, CancellationToken cancellationToken)
    {
        try
        {
            var teacher = await _unitOfWork.TeacherRepository.GetById(request.teacherId);
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            if (course != null && teacher != null)
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.TeacherRepository.AssignToCourse(course, teacher);
                _logger.LogInformation($"Action in teacher at: {DateTime.Now.TimeOfDay}");

                // Commit the transaction to save changes
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<TeacherDto>(teacher);
            }
            throw new TeacherNotFoundException("Teacher or course not found");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in teacher at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

}
