using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ICourseRepository
{

    public Course Create(Course course);
    public int GetLastId();

    public Course? GetById(int id);

    public void UpdateCourse(Course course, int id);

    public void DeleteCourse(int id);

    public void Add(Student s,int courseId);
}
