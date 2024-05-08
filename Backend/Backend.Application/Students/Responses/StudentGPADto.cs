using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Students.Responses;

public class StudentGPADto
{
    //public int StudentId { get; set; }
    //public StudentDto? Student { get; set; }
    public int CourseId { get; set; }
    [JsonIgnore]
    public CourseDto? Course { get; set; }
    public String? CourseName { get; set; }
    public decimal GPAValue { get; set; }

    public static StudentGPADto FromStudentGpa(StudentGPA studentGPA)
    {
        return new StudentGPADto
        {
            //StudentId = studentGPA.StudentId,
            //Student = StudentDto.FromStudent(studentGPA.Student),
            CourseId = studentGPA.CourseId,
            GPAValue = studentGPA.GPAValue,
            CourseName = studentGPA.Course?.Name,
            //CourseN = CourseDto.FromCourse(studentGPA.Course)
        };
    }
}
