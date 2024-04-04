using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface IStudentRepository
{

    public Student Create(Student student);
    int GetLastId();
    public void MotivateAbsence(DateTime date, Course course, Student student);
    public Student? GetById(int id);

    public Student? GetByName(string name); 
    public void Delete(Student student);
    public void RemoveGrade(Student student, Course course, int grade);
    public void AddAbsence(Student student,Absence absence);
    public Student UpdateStudent(Student student,int id);
    public void AddGrade(int grade, Student s, Course c);
    public void EnrollIntoCourse(Student s, Course c);

}
