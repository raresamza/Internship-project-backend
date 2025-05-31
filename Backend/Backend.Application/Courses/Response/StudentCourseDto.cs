using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Response;

public class StudentCourseDto
{
    public int StudentId { get; set; }

    [JsonIgnore]
    public StudentDto Student { get; set; }

    public float ParticipationPoints { get; set; }

    //public int CourseId { get; set; }
    public string? StudentName { get; set; }
    //public string? CourseName { get; set; }

    //public CourseDto? Course { get; set; }

    public static StudentCourseDto FromStudentCourse(StudentCourse studentCourseDto)
    {
        return new StudentCourseDto
        {
            StudentId = studentCourseDto.StudentId,
            //CourseId = studentCourseDto.CourseId,
            StudentName = studentCourseDto.Student.Name,
            ParticipationPoints = studentCourseDto.ParticipationPoints,
            //CourseName = studentCourseDto.Course?.Name
            //Student = StudentDto.FromStudent(studentCourseDto.Student),
            //Course = CourseDto.FromCourse(studentCourseDto.Course),
        };
    }
}
