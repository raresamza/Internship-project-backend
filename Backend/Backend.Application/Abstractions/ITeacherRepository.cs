using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ITeacherRepository
{

    public Teacher Create(Teacher teacher);
    public Teacher GetTeacherById(int teacherId);
    public void DeleteTeacherById(int teacherId);
    public List<Teacher> GetAllTeachers();
    public void UpdateTeacher(int teacherID);
    public void AddTeacher(Teacher teacher);
}
