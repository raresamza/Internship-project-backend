using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ISchoolRepository
{
    public void AddClassroom(Classroom classroom, School school);
    public void RemoveClassroom(Classroom classroom, School school);
    int GetLastId();

    public IEnumerable<School> GetAll();
    public School Create(School school);
    public School? GetById(int id);
    public School Update(int schoolId, School school);
    public void Delete(School school);
}
