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
                teacher.TaughtCourse = course;
                teacher.TaughtCourseId = course.ID;
                await _unitOfWork.TeacherRepository.UpdateTeacher(teacher, teacher.ID);
                course.Teacher = teacher;
                course.TeacherId = teacher.ID;
                await _unitOfWork.CourseRepository.UpdateCourse(course, course.ID);
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation($"Action in teacehr at: {DateTime.Now.TimeOfDay}");
                //return TeacherDto.FromTeacher(teacher);
                return _mapper.Map<TeacherDto>(teacher);
            }
            throw new TeacherNotFoundException("S");
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
