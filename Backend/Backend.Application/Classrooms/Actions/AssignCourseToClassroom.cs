using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.CourseException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;

public record AssignCourseToClassroom(int courseId,int classroomId):IRequest<ClassroomDto>;
public class AssignCourseToClassroomHadnler : IRequestHandler<AssignCourseToClassroom, ClassroomDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AssignCourseToClassroomHadnler> _logger;
    public AssignCourseToClassroomHadnler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AssignCourseToClassroomHadnler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ClassroomDto> Handle(AssignCourseToClassroom request, CancellationToken cancellationToken)
    {
        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);
            if (course == null)
            {
                throw new NullCourseException($"Course with id: {request.courseId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"Classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.ClassroomRepository.AssignCourse(course, classroom);
            await _unitOfWork.ClassroomRepository.UpdateClassroom(classroom, classroom.ID);
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
