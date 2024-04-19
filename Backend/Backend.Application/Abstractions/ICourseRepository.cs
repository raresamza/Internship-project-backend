using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ICourseRepository
{

    public Course Create(Course course);
    //public int GetLastId();

    public Course? GetById(int id);

    public Course UpdateCourse(Course course, int id);

    public void DeleteCourse(Course course);

    public void Add(Student s,int courseId);
}
