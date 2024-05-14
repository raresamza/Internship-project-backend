using AutoMapper;
using Backend.Application.Absences.Create;
using Backend.Application.Abstractions;
using Backend.Application.Schools.Update;
using Backend.Application.Students.Create;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backend.Application.Students.Actions;

public record AddAbsence(int studentId, int absenceId) : IRequest<StudentDto>;
public class AddAbsenceHandler : IRequestHandler<AddAbsence, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddAbsenceHandler> _logger;
    public AddAbsenceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddAbsenceHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StudentDto> Handle(AddAbsence request, CancellationToken cancellationToken)
    {

        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            var absence = await _unitOfWork.AbsenceRepository.GetById(request.absenceId);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
            }
            if (absence == null)
            {
                throw new InvalidAbsenceException($"Absence with id: {request.absenceId} was not found");
            }

            await _unitOfWork.BeginTransactionAsync();//move
            _unitOfWork.StudentRepository.AddAbsence(student, absence);
            await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in students at: {DateTime.Now.TimeOfDay}");
            //return StudentDto.FromStudent(student);
            return _mapper.Map<StudentDto>(student);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in students at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }

}
