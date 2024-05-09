using AutoMapper;
using Backend.Application.Absences.Delete;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Castle.Core.Logging;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Queries;

public record GetAbsenceByDateAndCourse(DateTime Date, int courseId, int studentId) : IRequest<AbsenceDto>;
public class GetAbsenceByDateAndCourseHandler : IRequestHandler<GetAbsenceByDateAndCourse, AbsenceDto>
{


    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAbsenceByDateAndCourseHandler> _logger;
    public GetAbsenceByDateAndCourseHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAbsenceByDateAndCourseHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AbsenceDto> Handle(GetAbsenceByDateAndCourse request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            if (student == null)
            {
                throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
            }
            if (course == null)
            {
                throw new NullCourseException($"The course with id: {request.courseId} was not found");
            }
            var absence = await _unitOfWork.AbsenceRepository.GetByDateAndCourse(request.Date, course, student);
            if (absence == null)
            {
                throw new TeacherNotFoundException($"The absence for the course: {request.courseId}, on date: {request.Date} was not found!");
            }
            //return AbsenceDto.FromAbsence(absence);
            _logger.LogError($"Absence action executed at: {DateTime.Now.TimeOfDay}");
            return _mapper.Map<AbsenceDto>(absence);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in absence at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
