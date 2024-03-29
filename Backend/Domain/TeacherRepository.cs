using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using MediatR;
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

    public Teacher? GetById(int id)
    {
        return _teachers.FirstOrDefault(s => s.ID == id);
    }

    public int GetLastId()
    {
        if (_teachers.Count == 0) return 1;
        var lastId = _teachers.Max(a => a.ID);
        return lastId + 1;
    }
}

