using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Response;

public class CourseDto
{

    public required string Name { get; set; }
    public required Subject Subject { get; set; }

    public int ID { get; set; }
    public ICollection<StudentCourse> StudentCourses { get; set; }
    public int? TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public static CourseDto FromCourse(Course course)
    {
        return new CourseDto
        {
            ID = course.ID,
            Subject = course.Subject,
            Name = course.Name,
            StudentCourses = course.StudentCourses,
            Teacher = course.Teacher,
            TeacherId= course.TeacherId
        };
    }

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
            sb.Append($"\t\t{student.Student.Name}");
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
