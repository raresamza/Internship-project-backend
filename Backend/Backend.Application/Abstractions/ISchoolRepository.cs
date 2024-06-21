using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ISchoolRepository
{
    public void AddClassroom(Classroom classroom, UpdateSchoolDto school);
    public void RemoveClassroom(Classroom classroom, UpdateSchoolDto school);
    //int GetLastId();

    public Task<List<UpdateSchoolDto>> GetAll();
    public Task<UpdateSchoolDto> Create(UpdateSchoolDto school);
    public Task<UpdateSchoolDto?> GetById(int id);
    public Task<UpdateSchoolDto> Update(int schoolId, UpdateSchoolDto school);
    public Task Delete(UpdateSchoolDto school);
}
