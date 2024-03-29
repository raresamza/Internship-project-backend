using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
namespace Backend.Infrastructure;
public class TeacherRepository : ITeacherRepository
{

    private readonly List<Teacher> _teachers = new();


    //Db mock
    public void AddTeacher(Teacher teacher)
    {
        _teachers.Add(teacher);
    }

    public void DeleteTeacherById(int teacherId)
    {
        _teachers.Remove(_teachers.First(teacher => teacher.ID == teacherId));
    }

    public List<Teacher> GetAllTeachers()
    {
        return _teachers;
    }

    public Teacher GetTeacherById(int teacherId)
    {
        if (!_teachers.Any(teacher => teacher.ID == teacherId))
        {
            throw new StudentNotFoundException($"Student with ID: {teacherId} not found!");
        }
        return _teachers.First(teacher => teacher.ID == teacherId);
    }

    public void UpdateTeacher(int studentID)
    {
        throw new NotImplementedException();
    }

    public Teacher Create(Teacher teacher)
    {
        _teachers.Add(teacher);
        return teacher;
    }
}

