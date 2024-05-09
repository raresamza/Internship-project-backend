using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Responses;

public class TeacherDto
{
    public int ID { get; set; }

    public required int Age { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required int PhoneNumber { get; set; }
    public required Subject Subject { get; set; }

    //[JsonIgnore]
    //public CourseDto? TaughtCourse { get; set; }
    public string? CourseName { get; set; }
    public CourseDto? TaughtCourse { get; set; }

    public ICollection<StudentCourseDto>? StudentCourses { get; set; }
    public int? TaughtCourseId { get; set; }
    public static TeacherDto FromTeacher(Teacher teacher)
    {
        Console.WriteLine(teacher);
        return new TeacherDto
        {
            TaughtCourseId = teacher.TaughtCourse.TeacherId,
            Address = teacher.Address,
            Age = teacher.Age,
            Name = teacher.Name,
            PhoneNumber = teacher.PhoneNumber,
            Subject = teacher.Subject,
            ID = teacher.ID,
            StudentCourses = teacher.TaughtCourse.StudentCourses.Select((studentCourse) => StudentCourseDto.FromStudentCourse(studentCourse)).ToList(),
            TaughtCourse = CourseDto.FromCourse(teacher.TaughtCourse),
            CourseName = teacher.TaughtCourse.Name,
        };
    }
    public override string ToString()
    {

        StringBuilder stringBuilder = new StringBuilder();
        if (TaughtCourse != null)
        {
            stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject}\n\tTaught Course: {TaughtCourse.ToString()}");

        }
        else
        {
            stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject}\n\tTaught Course: !Coruse is not assigned yet!\n");

        }
        return stringBuilder.ToString();
    }
}
