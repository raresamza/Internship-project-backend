using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Backend.Domain.Models;
public class Course
{
    public Course(string name, Subject subject)
    {
        Name = name;
        Subject = subject;
    }

    public Course()
    {
        StudentCourses = _students;
        Grades = _grades;
        ClassroomCourses = _classroomCourses;
        GPAs = _gpas;
    }


    private readonly List<StudentGPA> _gpas = new();
    public ICollection<StudentGPA> GPAs { get; set; }

    public required string Name { get; set; }
    public required Subject Subject { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public int? TeacherId { get; set; }

    public Teacher Teacher { get; set; }


    public ICollection<StudentCourse> StudentCourses { get; set; }
    private readonly List<StudentCourse> _students = new();


    public ICollection<StudentGrade> Grades { get; set; }
    private readonly List<StudentGrade> _grades = new();



    public ICollection<ClassroomCourse> ClassroomCourses { get; set; }
    private readonly List<ClassroomCourse> _classroomCourses = new();

    public int ID { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (Teacher != null)
        {
            sb.Append($"The course {Name} has the teacher {Teacher.Name} and {StudentCourses.Count} students and the list of students enrolled is:\n");

        }
        else
        {
            sb.Append($"The course \"{Name}\" is currently uninitilized, please proceed to do so.");
            return sb.ToString();
        }
        foreach (StudentCourse student in StudentCourses)
        {
            sb.Append($"\t\t{student.Student?.Name}");
            sb.Append("\n");
        }
        return sb.ToString();
    }
}