using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
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

        if(dbCourse == null )
        {
            throw new NullCourseException($"The course with id: {request.courseId} was not found");
        }

        if (dbStudent == null)
        {
            throw new StudentNotFoundException($"The student with id: {request.studentId} was not found");
        }

        if (dbCourse.StudentCourses.Any(sc => sc.Student == dbStudent))
        {
            throw new StudentException($"Student {dbStudent?.Name} is already enrolled into this course");
        }
        var studentCourse = new StudentCourse { Student = dbStudent, Course = dbCourse, StudentId =dbStudent.ID, CourseId=dbCourse.ID };
        dbCourse.StudentCourses.Add(studentCourse);
        List<int> grades = new List<int>();
        
        dbStudent.GPAs.Add(new StudentGPA
        {
            StudentId = dbStudent.ID,
            CourseId = dbCourse.ID,
            GPAValue = 0,
            Student=dbStudent,
            Course=dbCourse,
        });
        dbStudent.Grades.Add(new StudentGrade
        {
            StudentId = dbStudent.ID,
            CourseId = dbCourse.ID,
            GradeValues = grades,
            Student=dbStudent,
            Course =dbCourse,
        });
        //_courseRepository.UpdateCourse(dbCourse, dbCourse.ID);
        _studentRepository.UpdateStudent(dbStudent, dbStudent.ID);
        return Task.FromResult(CourseDto.FromCourse(dbCourse));
    }
}
