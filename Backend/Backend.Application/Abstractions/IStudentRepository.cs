using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface IStudentRepository
{

    public Task<Student> Create(Student student);

    public Task<List<Student>> GetAll();
    //int GetLastId();
    public void MotivateAbsence(DateTime date, Course course, Student student);
    public Task<Student?> GetById(int id);

    public Task<Student?> GetByName(string name); 
    public Task Delete(Student student);
    public void RemoveGrade(Student student, Course course, int grade);
    public void AddAbsence(Student student,Absence absence);
    public Task<Student> UpdateStudent(Student student,int id);
    public void AddGrade(int grade, Student s, Course c);
    public void EnrollIntoCourse(Student s, Course c);

}
