using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Response;

public class ClassroomDto
{
    public ICollection<ClassroomCourse> ClassroomCourses { get; set; }

    public int ID { get; set; }
    public string Name { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<TeacherClassroom> Teachers { get; set; }

    public static ClassroomDto FromClassroom(Classroom classroom)
    {
        return new ClassroomDto
        {
            ID= classroom.ID,
            ClassroomCourses = classroom.ClassroomCourses,
            Students = classroom.Students,
            Teachers = classroom.Teachers,
            Name = classroom.Name,
        };
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"This classrooms has the following teachers:\n");
        foreach (TeacherClassroom teacher in Teachers)
        {
            stringBuilder.Append($"\t{teacher.Teacher.ToString()}");
        }
        stringBuilder.Append($"This classrooms has the following students:");
        foreach (Student student in Students)
        {
            stringBuilder.Append($"\t{student.ToString()}");
        }
        return stringBuilder.ToString();
    }
}
