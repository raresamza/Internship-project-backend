using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface IStudentRepository
{

    public Student Create(Student student);
    int GetLastId();

    public Student? GetById(int id);


}
