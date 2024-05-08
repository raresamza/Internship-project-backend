﻿using Backend.Application.Absences.Create;
using Backend.Application.Abstractions;
using Backend.Application.Students.Create;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backend.Application.Students.Actions;

public record AddAbsence(int studentId, int absenceId) : IRequest<StudentDto>;
internal class AddAbsenceHandler : IRequestHandler<AddAbsence, StudentDto>
{

    private readonly IUnitOfWork _unitOfWork;

    public AddAbsenceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.StudentRepository.AddAbsence(student, absence);
            await _unitOfWork.StudentRepository.UpdateStudent(student, student.ID);
            await _unitOfWork.CommitTransactionAsync();

            return StudentDto.FromStudent(student);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

    }
}
