using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using MediatR;
using Backend.Exceptions.TeacherException;
namespace Backend.Infrastructure;
public class TeacherRepository : ITeacherRepository
{
    private readonly List<Teacher> _teachers = new();
    //Db mock
    public Teacher Create(Teacher teacher)
    {
        _teachers.Add(teacher);
        return teacher;
    }

    public void Delete(int id)
    {
        var teacher = GetById(id);
        _teachers.Remove(teacher);
    }

    public Teacher? GetById(int id)
    {
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
            throw new TeacherNotFoundException($"Teacher with id {id} not found");
        }
        oldTeacher = teacher;
    }
}

