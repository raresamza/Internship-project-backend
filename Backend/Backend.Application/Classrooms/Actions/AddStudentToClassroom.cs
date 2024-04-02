using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Students.Responses;
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

        _classroomRepository.AddStudent(student, classroom);
        _classroomRepository.UpdateClassroom(classroom, classroom.ID);
        _studentRepository.UpdateStudent(student, classroom.ID);

        return Task.FromResult(StudentDto.FromStudent(student));
    }
}
