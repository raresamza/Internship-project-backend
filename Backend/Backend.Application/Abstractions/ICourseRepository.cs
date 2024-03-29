using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface ICourseRepository
{

    public Course Create(Course course);
    public int GetLastId();
}
