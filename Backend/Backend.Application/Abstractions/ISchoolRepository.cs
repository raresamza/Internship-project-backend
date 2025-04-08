using Backend.Application.Schools.Response;
using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ISchoolRepository
{
    public void AddClassroom(Classroom classroom, School school);
    public void RemoveClassroom(Classroom classroom, School school);
    //int GetLastId();


    public Task<List<School>> GetAll();
    public Task<School> Create(School school);
    public Task<School?> GetById(int id);
    public Task<School> Update(int schoolId, SchoolUpdateDto school);
    public Task Delete(School school);
}
