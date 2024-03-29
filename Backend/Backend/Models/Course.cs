using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;
public class Course
{
    public Course(string name, Subject subject)
    {
        ID = ++_lastAssignedId;
        Name = name;
        Subject = subject;
    }

    public Course()
    {
        ID = ++_lastAssignedId;
        Students = _students;
    }
    //enroll move
    public required string Name { get; set; }
    public required Subject Subject { get; set; }
    public Teacher Teacher { get; set; }
    private readonly List<Student> _students = new();

    public ICollection<Student> Students { get; set; }

    private static int _lastAssignedId = 0;

    public int ID { get; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (Teacher != null)
        {
            sb.Append($"The course {Name} has the teacher {Teacher.Name} and {Students.Count} students and the list of students enrolled is:\n");

        }
        else
        {
            sb.Append($"The course \"{Name}\" is currently uninitilized, please proceed to do so.");
            return sb.ToString();
        }
        foreach (Student student in Students)
        {
            sb.Append($"\t\t{student.Name}");
            sb.Append("\n");
        }
        return sb.ToString();
    }
}