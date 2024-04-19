

namespace Backend.Domain.Models;

public class StudentGrade
{
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public List<int> GradeValues { get; set; } = new();
}
