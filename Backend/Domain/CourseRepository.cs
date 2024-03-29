using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Exceptions.TeacherException;


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
        return _courses.FirstOrDefault(c => c.ID == id);
    }

    public void UpdateCourse(Course course, int id)
    {
        var oldCoruse = GetById(id);
        if (oldCoruse == null)
        {
            throw new TeacherNotFoundException($"Teacher with id {id} not found");
        }
        oldCoruse = course;
    }

    public void DeleteCourse(int id)
    {
        var course = GetById(id);
        _courses.Remove(course);
    }

    public void Add(Student student, int s)
    {
        var course = GetById(student.ID);
        course.Students.Add(student);
    }
}
