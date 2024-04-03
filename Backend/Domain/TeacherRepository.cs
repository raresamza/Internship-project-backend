using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using MediatR;
using Backend.Exceptions.TeacherException;
using Backend.Application.Courses.Actions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.Placeholders;
namespace Backend.Infrastructure;
public class TeacherRepository : ITeacherRepository
{
    private readonly List<Teacher> _teachers = new();

    public void AssignToCourse(Course course, Teacher teacher)
    {
        if (teacher.Subject == course.Subject)
        {
            course.Teacher = teacher;
            teacher.TaughtCourse = course;
        }
        else
        {
            TeacherException.LogError();
            throw new TeacherSubjectMismatchException($"The subject that the teacher spelcializes in: {teacher.Subject} does not match with the course subject: {course.Subject}");
        }
    }

    //Db mock
    public Teacher Create(Teacher teacher)
    {
        _teachers.Add(teacher);
        Logger.LogMethodCall(nameof(Create), true);
        return teacher;
    }

    public void Delete(int id)
    {
        var teacher = GetById(id);
        _teachers.Remove(teacher);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Teacher? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _teachers.FirstOrDefault(t => t.ID == id);
    }

    public int GetLastId()
    {
        if (_teachers.Count == 0) return 1;
        var lastId = _teachers.Max(a => a.ID);
        return lastId + 1;
    }

    public void UpdateTeacher(Teacher teacher, int id)
    {
        var oldTeacher = GetById(id);
        if (oldTeacher == null)
        {
            Logger.LogMethodCall(nameof(UpdateTeacher), false);
            throw new TeacherNotFoundException($"Teacher with id {id} not found");
        }
        oldTeacher = teacher;
        Logger.LogMethodCall(nameof(UpdateTeacher), true);
    }
}

