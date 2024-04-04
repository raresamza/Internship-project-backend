using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Exceptions.TeacherException;
using Backend.Application.Courses.Actions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;


namespace Backend.Infrastructure;
public class CourseRepository : ICourseRepository
{

    private readonly List<Course> _courses = new();

    public Course Create(Course course)
    {
        _courses.Add(course);
        return course;
    }

    //Db mock
    //public Course GetCourseById(int courseId, List<Course> courses)
    //{
    //    if (!_courses.Any(course => course.ID == courseId))
    //    {
    //        throw new StudentNotFoundException($"Student with ID: {courseId} not found!");
    //    }
    //    //return null;
    //    return _courses.First(course => course.ID == courseId);
    //}

    public int GetLastId()
    {
        if (_courses.Count == 0) return 1;
        var lastId = _courses.Max(a => a.ID);
        return lastId + 1;
    }

    public Course? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _courses.FirstOrDefault(c => c.ID == id);
    }

    public Course UpdateCourse(Course course, int id)
    {
        var oldCourse = _courses.FirstOrDefault(s => s.ID == id);
        if (oldCourse != null)
        {
            oldCourse = course;

            return oldCourse;
        }
        else
        {
            throw new NullCourseException($"The course with id: {id} was not found");
        }
    }

    public void DeleteCourse(Course course)
    {
        _courses.Remove(course);
        Logger.LogMethodCall(nameof(DeleteCourse), true);
    }

    public void Add(Student student, int s)
    {
        var course = GetById(student.ID);
        course.Students.Add(student);
        Logger.LogMethodCall(nameof(Add), true);
    }
}
