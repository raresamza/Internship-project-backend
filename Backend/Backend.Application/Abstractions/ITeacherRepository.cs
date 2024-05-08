using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ITeacherRepository
{

    public Task<Teacher> Create(Teacher teacher);
    //int GetLastId();
    public Task<List<Teacher>> GetAll();   
    public Task<Teacher?> GetById(int id);

    public Task<Teacher> UpdateTeacher(Teacher teacher,int id);
    public void AssignToCourse(Course course, Teacher teacher);

    public Task Delete(Teacher teacher);    
}
