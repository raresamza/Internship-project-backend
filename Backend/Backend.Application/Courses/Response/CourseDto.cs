using Backend.Application.Students.Responses;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Response;

public class CourseDto
{

    public required string Name { get; set; }
    public required Subject Subject { get; set; }

    public int ID { get; set; }

    //public ICollection<StudentCourseDto> StudentCourses { get; set; }
    //[JsonIgnore]
    public ICollection<StudentCourseDto>? StudentCourses { get; set; }

    public int? TeacherId { get; set; }
    public String? TeacherName { get; set; }

    //public int StudentCount {  get; set; }
    //[JsonIgnore]
    //public TeacherDto? Teacher { get; set; }

    public static CourseDto FromCourse(Course course)
    {
        //Console.WriteLine(course.Teacher);
        return new CourseDto
        {
            ID = course.ID,
            TeacherName = course.Teacher != null ? course.Teacher.Name : "Unknown",
            Subject = course.Subject,
            Name = course.Name,
            StudentCourses = course.StudentCourses.Select(studentCourse => StudentCourseDto.FromStudentCourse(studentCourse)).ToList(),
            //StudentCourses = course.StudentCourses,
            //Teacher = TeacherDto.FromTeacher(course.Teacher),
            TeacherId = course.TeacherId
        };
        
    }

    //public override string ToString()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    if (Teacher != null)
    //    {
    //        sb.Append($"The course {Name} has the teacher {Teacher.Name} and {StudentCourses.Count} students and the list of students enrolled is:\n");

    //    }
    //    else
    //    {
    //        sb.Append($"The course \"{Name}\" is currently uninitilized, please proceed to do so.");
    //        return sb.ToString();
    //    }
    //    foreach (StudentCourseDto student in StudentCourses)
    //    {
    //        sb.Append($"\t\t{student.Student.Name}");
    //        sb.Append("\n");
    //    }
    //    return sb.ToString();
    //}
}
