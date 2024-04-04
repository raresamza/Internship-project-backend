using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ITeacherRepository
{

    public Teacher Create(Teacher teacher);
    int GetLastId();

    public Teacher? GetById(int id);

    public Teacher UpdateTeacher(Teacher teacher,int id);
    public void AssignToCourse(Course course, Teacher teacher);

    public void Delete(Teacher teacher);    
}
