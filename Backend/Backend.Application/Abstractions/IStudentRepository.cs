using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface IStudentRepository
{

    public Student Create(Student student);
    public Student GetStudentById(int studentId);
    public void DeleteStudentById(int studentId);
    public List<Student> GetAllStudents();
    public void UpdateStudent(int studentID);
    public void AddStudent(Student student);

}
