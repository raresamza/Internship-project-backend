using Backend.Application.Abstractions;
using Backend.Application.Students.Responses;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Actions;

public record AddGpa(int studentId, int courseId) : IRequest<StudentDto>;
public class AddGpaHandler : IRequestHandler<AddGpa, StudentDto>
{

    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICatalogueRepository _catalogueRepository;

    public AddGpaHandler(IStudentRepository studentRepository, ICourseRepository courseRepository, ICatalogueRepository catalogueRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _catalogueRepository= catalogueRepository;
    }

    Task<StudentDto> IRequestHandler<AddGpa, StudentDto>.Handle(AddGpa request, CancellationToken cancellationToken)
    {
        var course=_courseRepository.GetById(request.courseId);
        var student=_studentRepository.GetById(request.studentId);
        if (course == null)
        {
            throw new NullCourseException($"Course with id: {request.courseId} was not found");
        }
        if(student == null)
        {
            throw new StudentNotFoundException($"Student with id: {request.studentId} was not found");
        }

        _catalogueRepository.AddGpa(course, student);
        _studentRepository.UpdateStudent(student,student.ID);

        return Task.FromResult(StudentDto.FromStudent(student));

    }
}
