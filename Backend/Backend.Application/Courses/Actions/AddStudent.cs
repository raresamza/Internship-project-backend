using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Exceptions.Placeholders;
using MediatR;


namespace Backend.Application.Courses.Actions;

public record AddStudent(int studentId,int courseId) : IRequest<CourseDto>;
public class AddStudentHandler : IRequestHandler<AddStudent, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;

    public AddStudentHandler(ICourseRepository courseRepository,IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public Task<CourseDto> Handle(AddStudent request, CancellationToken cancellationToken)
    {
        var dbCourse=_courseRepository.GetById(request.courseId);
        var dbStudent=_studentRepository.GetById(request.studentId);

        ;
        if (dbCourse.Students.Contains(dbStudent))
        {
            throw new StudentException($"Student {dbStudent.Name} is already enrolled into this course");
        }
        dbCourse.Students.Add(dbStudent);
        List<int> grades = new List<int>();
        
        dbStudent.GPAs.Add(dbCourse, 0);
        dbStudent.Grades.Add(dbCourse, grades);
        _courseRepository.UpdateCourse(dbCourse, dbCourse.ID);
        _studentRepository.UpdateStudent(dbStudent, dbStudent.ID);

        return Task.FromResult(CourseDto.FromCourse(dbCourse));
    }
}
