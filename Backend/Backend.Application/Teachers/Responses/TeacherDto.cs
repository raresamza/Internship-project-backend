using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public Course TaughtCourse { get; set; }
    public int? TaughtCourseId { get; set; }
    public static TeacherDto FromTeacher(Teacher teacher)
    {
        return new TeacherDto
        {

            Address = teacher.Address,
            Age = teacher.Age,
            Name = teacher.Name,
            PhoneNumber = teacher.PhoneNumber,
            Subject = teacher.Subject,
            ID = teacher.ID,
            TaughtCourse = teacher.TaughtCourse,
            TaughtCourseId = teacher.TaughtCourseId,
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
