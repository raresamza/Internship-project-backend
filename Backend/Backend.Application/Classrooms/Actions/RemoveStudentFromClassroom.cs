﻿using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;


public record RemoveStudentFromClassroom(int studentId, int classroomId) : IRequest<ClassroomDto>;

public class RemoveStudentFromClassroomHandler : IRequestHandler<RemoveStudentFromClassroom, ClassroomDto>
{

    private readonly IStudentRepository _studentRepository;
    private readonly IClassroomRepository _classroomRepository;

    public RemoveStudentFromClassroomHandler(IStudentRepository studentRepository, IClassroomRepository classroomRepository)
    {
        _studentRepository = studentRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<ClassroomDto> Handle(RemoveStudentFromClassroom request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var classroom = _classroomRepository.GetById(request.classroomId);

        if (student == null)
        {
            throw new StudentNotFoundException($"Teacher with id: {request.studentId} was not found");
        }
        if (classroom == null)
        {
            throw new NullClassroomException($"The classroom with id: {request.classroomId} was not found");
        }

        _classroomRepository.RemoveStudent(student, classroom);
        _classroomRepository.UpdateClassroom(classroom, classroom.ID);

        return Task.FromResult(ClassroomDto.FromClassroom(classroom));
    }
}