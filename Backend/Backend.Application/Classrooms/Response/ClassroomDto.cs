using Backend.Application.Students.Responses;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Response;

public class ClassroomDto
{
    public ICollection<ClassroomCourseDto> ClassroomCourses { get; set; }

    public int ID { get; set; }
    public string Name { get; set; }
    //[JsonIgnore]

    public ICollection<StudentDto> Students { get; set; }
    //public ICollection<StudentDto> Students { get; set; }
    [JsonIgnore]
    public ICollection<TeacherClassroomDto> Teachers { get; set; }
    //public ICollection<TeacherClassroomDto> Teachers { get; set; }

    public static ClassroomDto FromClassroom(Classroom classroom)
    {
        return new ClassroomDto
        {
            ID = classroom.ID,
            ClassroomCourses = classroom.ClassroomCourses.Select((classroomCourse) => ClassroomCourseDto.FromClassroomCourse(classroomCourse)).ToList(),
            Students = classroom.Students.Select((student) => StudentDto.FromStudent(student)).ToList(),
            //Students = classroom.Students.Select((student) => StudentDto.FromStudent(student)).ToList(),
            //Teachers = classroom.Teachers.Select((teacher) => TeacherClassroomDto.FromTeacherClassroom(teacher)).ToList(),
            Teachers = classroom.Teachers.Select((classroomTeacher) => TeacherClassroomDto.FromTeacherClassroom(classroomTeacher)).ToList(),
            Name = classroom.Name,
        };
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"This classrooms has the following teachers:\n");
        foreach (TeacherClassroomDto teacher in Teachers)
        {
            stringBuilder.Append($"\t{teacher.Teacher.ToString()}");
        }
        stringBuilder.Append($"This classrooms has the following students:");
        foreach (StudentDto student in Students)
        {
            stringBuilder.Append($"\t{student.ToString()}");
        }
        return stringBuilder.ToString();
    }
}
