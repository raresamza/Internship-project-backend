using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface IStudentRepository
{

    public Student Create(Student student);
    int GetLastId();

    public Student? GetById(int id);

    public void Delete(int id);

    public void AddAbsence(Student student,Absence absence);

    public void UpdateStudent(Student student,int id);

    public void AddGrade(int grade, Student s, Course c);
    public void EnrollIntoCourse(Student s, Course c);

}
