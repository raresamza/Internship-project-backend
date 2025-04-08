using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ITeacherRepository
{

    public Task<Teacher> Create(Teacher teacher);
    //int GetLastId();
    public Task<List<Teacher>> GetAll();   
    public Task<Teacher?> GetById(int id);

    public Task<Teacher> UpdateTeacher(TeacherUpdateDto teacher,int id);
    public Task<Teacher> AssignToCourse(Course course, Teacher teacher);

    public Task Delete(Teacher teacher);
    public Task<Teacher?> GetByName(string name);
}
