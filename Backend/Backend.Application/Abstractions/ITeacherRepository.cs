using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ITeacherRepository
{

    public Teacher Create(Teacher teacher);
    int GetLastId();

    public Teacher? GetById(int id);
}
