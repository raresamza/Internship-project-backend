using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;

namespace Backend.Application.Abstractions;
public interface IClassroomRepository
{
    public Task<Classroom> Create(Classroom classroom);

    public Task<List<Classroom>> GetAll();
    //int GetLastId();
    public Task<Classroom?> GetById(int id);
    public Task Delete(Classroom classroom);
    public Task<Classroom> UpdateClassroom(ClassroomUpdateDto classroom, int id);
    public void AddTeacher(Teacher teacher, Classroom classroom);
    public void RemoveStudent(Student student, Classroom classroom);
    public void RemoveTeacher(Teacher teacher, Classroom classroom);
    public void AssignCourse(Course course, Classroom classroom);
    public void AddStudent(Student student, Classroom classroom);
}

