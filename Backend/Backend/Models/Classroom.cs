using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;

namespace Backend.Domain.Models;

public class Classroom
{

    public required string Name { get; set; }

    public School School { get; set; }

    public int SchoolId { get; set; }

    private List<ClassroomCourse> _classroomCourses = new();
    private List<TeacherClassroom> _teachers= new();
    private List<Student> _students = new();
    public Classroom(string name)
    {
        Name = name;
    }

    public Classroom() 
    {
        ClassroomCourses = _classroomCourses;
        Teachers = _teachers;
        Students = _students;
        
    }

    public Catalogue Catalogue { get; set; }

    public ICollection<ClassroomCourse> ClassroomCourses { get; set; }

    public int ID { get; set; } 
    public ICollection<Student> Students { get; set; } 
    public ICollection<TeacherClassroom> Teachers { get; set; } 

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"This classrooms has the following teachers:\n");
        foreach (TeacherClassroom teacher in Teachers)
        {
            stringBuilder.Append($"\t{teacher.ToString()}");
        }
        stringBuilder.Append($"This classrooms has the following students:");
        foreach (Student student in Students)
        {
            stringBuilder.Append($"\t{student.ToString()}");
        }
        return stringBuilder.ToString();
    }
}
