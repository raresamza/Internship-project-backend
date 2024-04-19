using Backend.Domain.Models;

namespace Backend.Application.Abstractions;
public interface IClassroomRepository
{
    public Classroom Create(Classroom classroom);
    //int GetLastId();
    public Classroom? GetById(int id);
    public void Delete(Classroom classroom);
    public Classroom UpdateClassroom(Classroom classroom, int id);
    public void AddTeacher(Teacher teacher, Classroom classroom);
    public void RemoveStudent(Student student, Classroom classroom);
    public void RemoveTeacher(Teacher teacher, Classroom classroom);
    public void AssignCourse(Course course, Classroom classroom);
    public void AddStudent(Student student, Classroom classroom);
}

