using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ISchoolRepository
{
    public void AddClassroom(Classroom classroom, School school);
    public void RemoveClassroom(Classroom classroom, School school);
    int GetLastId();
    public School Create(School school);
    public School? GetById(int id);

    public void UpdateSchool(School school, int id);
}
