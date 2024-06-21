using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Actions;

public record UndoGpa(int studentId, int courseId) : IRequest<StudentDto>;
public class UndoGpaHandler : IRequestHandler<UndoGpa, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UndoGpaHandler> _logger;
    public UndoGpaHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UndoGpaHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<StudentDto> IRequestHandler<UndoGpa, StudentDto>.Handle(UndoGpa request, CancellationToken cancellationToken)
    {

        try
        {
            var course = await _unitOfWork.CourseRepository.GetById(request.courseId);
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            if (course == null)
            {
                throw new NullCourseException($"Course with id: {request.courseId} was not found");
            }
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
            }


            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.CatalogueRepository.UndoGpa(course, student);
            //await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Catalogue action executed at: {DateTime.Now.TimeOfDay}");

            //return StudentDto.FromStudent(student);
            return _mapper.Map<StudentDto>(student);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in catalogue at: {DateTime.Now.TimeOfDay}");
            await Console.Out.WriteLineAsync(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
