using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using MediatR;
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

    public GetAbsenceByDateAndCourseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            return AbsenceDto.FromAbsence(absence);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
}
