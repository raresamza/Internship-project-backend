using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Students.Responses;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Actions;
public record AddStudentToClassroom(int studentId, int classroomId) : IRequest<StudentDto>;
public class AddStudentToClassroomHandler : IRequestHandler<AddStudentToClassroom, StudentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IClassroomRepository _classroomRepository;

    public AddStudentToClassroomHandler(IStudentRepository studentRepository, IClassroomRepository classroomRepository)
    {
        _studentRepository = studentRepository;
        _classroomRepository = classroomRepository;
    }

    public Task<StudentDto> Handle(AddStudentToClassroom request, CancellationToken cancellationToken)
    {
        var student = _studentRepository.GetById(request.studentId);
        var classroom = _classroomRepository.GetById(request.classroomId);

        if(student == null)
        {
            throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
        }
        if (classroom == null) 
        {
            throw new NullClassroomException($"Classroom with id: {request.classroomId} was not found");
        }

        _classroomRepository.AddStudent(student, classroom);
        _classroomRepository.UpdateClassroom(classroom, classroom.ID);
        _studentRepository.UpdateStudent(student, student.ID);

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
