using AutoMapper;
using Backend.Application.Absences.Queries;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Students.Responses;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;
public record AddStudentToClassroom(int studentId, int classroomId) : IRequest<ClassroomDto>;
public class AddStudentToClassroomHandler : IRequestHandler<AddStudentToClassroom, ClassroomDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddStudentToClassroomHandler> _logger;
    public AddStudentToClassroomHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddStudentToClassroomHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClassroomDto> Handle(AddStudentToClassroom request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
            }
            if (classroom == null)
            {
                throw new NullClassroomException($"Classroom with id: {request.classroomId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.ClassroomRepository.AddStudent(student, classroom);
            //await _unitOfWork.ClassroomRepository.UpdateClassroom(classroom, classroom.ID);
            //await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in classroom at: {DateTime.Now.TimeOfDay}");

            //return StudentDto.FromStudent(student);
            return _mapper.Map<ClassroomDto>(classroom);
        } catch (Exception ex)
        {
            _logger.LogError($"Error in classroom at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
