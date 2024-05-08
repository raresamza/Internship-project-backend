using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ICourseRepository
{

    public Task<Course> Create(Course course);

    public Task<Course?> GetById(int id);
    public Task<List<Course>> GetAll();

    public Task<Course> UpdateCourse(Course course, int id);

    public Task DeleteCourse(Course course);

    public void Add(Student s,int courseId);
}
