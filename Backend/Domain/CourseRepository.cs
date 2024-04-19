using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Exceptions.TeacherException;
using Backend.Application.Courses.Actions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;


namespace Backend.Infrastructure;
public class CourseRepository : ICourseRepository
{

    //private readonly List<Course> _courses = new();
    private readonly AppDbContext _appDbContext;


    public CourseRepository(AppDbContext app)
    {
        _appDbContext = app;
    }

    public Course Create(Course course)
    {
        _appDbContext.Courses.Add(course);
        _appDbContext.SaveChanges();
        return course;
    }

    //Db mock
    public Course GetCourseById(int courseId, List<Course> courses)
    {
        var course = _appDbContext.Courses.FirstOrDefault(c => c.ID == courseId);

        if (course == null)
        {
            throw new StudentNotFoundException($"Course with ID: {courseId} not found!");
        }

        return course;
    }

    //public int GetLastId()
    //{
    //    if (_courses.Count == 0) return 1;
    //    var lastId = _courses.Max(a => a.ID);
    //    return lastId + 1;
    //}

    public Course? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _appDbContext.Courses
                       .Include(c => c.Teacher) 
                       .Include(c =>c.StudentCourses)
                            .ThenInclude(sc => sc.Student)
                       .FirstOrDefault(c => c.ID == id);
    }

    public Course UpdateCourse(Course course, int id)
    {
        var courseToUpdate = _appDbContext.Courses.FirstOrDefault(c => c.ID == course.ID);

        if (courseToUpdate == null)
        {
            throw new NullCourseException($"Course with ID: {course.ID} not found");
        }

        courseToUpdate.Name = course.Name;
        courseToUpdate.Subject = course.Subject;
        courseToUpdate.Teacher = course.Teacher;
        courseToUpdate.TeacherId= course.TeacherId;

        _appDbContext.SaveChanges();

        return courseToUpdate;
    }

    public void DeleteCourse(Course course)
    {
        _appDbContext.Courses.Remove(course);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(DeleteCourse), true);
    }

    public void Add(Student student, int s)
    {
        var course = GetById(student.ID);
        var studentCourse = new StudentCourse
        {
            Course = course,
            CourseId = course.ID,
            StudentId = student.ID,
            Student = student,
        };
        course.StudentCourses.Add(studentCourse);
        _appDbContext.StudentCourses.Add(studentCourse);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(Add), true);
    }
}
