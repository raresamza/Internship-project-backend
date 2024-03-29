using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Responses;

public class StudentDto
{
    public int ID { get; set; }

    public required string ParentEmail { get; set; }

    public required string ParentName { get; set; }

    public required int Age { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required int PhoneNumber { get; set; }

    public static StudentDto FromStudent(Student student)
    {
        return new StudentDto
        {
            ID = student.ID,
            ParentEmail = student.ParentEmail,
            ParentName = student.ParentName,
            Age = student.Age,
            Name = student.Name,
            Address = student.Address,
            PhoneNumber = student.PhoneNumber,
        };
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"\nStudent(ID: {ID}) details:\n\tStundent Name: {Name}\n\tStudent Age: {Age}\n\tStudent Phone Number: {PhoneNumber}\n\tStudent's Parent Name: {ParentName}\n\tStudent's Parent Email Addrees: {ParentEmail}\n\tStudent Address: {Address}\n\tStudent Grades:\n");
        //foreach (var entry in Grades)
        //{
        //    Course course = entry.Key;
        //    List<int> grades = entry.Value;
        //    stringBuilder.Append($"\t\tCourse: {course.Name}\n");

        //    stringBuilder.Append("\t\t\tGrades: ");
        //    foreach (var grade in grades)
        //    {
        //        stringBuilder.Append($"{grade} ");
        //    }
        //    stringBuilder.Append("\n\t\t\tGPA: ");
        //    stringBuilder.Append($"{(GPAs.TryGetValue(course, out decimal res) == true ? res : "N/A")} ");
        //    stringBuilder.Append('\n');

        //}
        //stringBuilder.Append($"\t\tAbsences:\n");

        //foreach (Absence absence in Absences)
        //{
        //    stringBuilder.Append($"\t\t\t{absence.Date.ToString("dd/MM/yyyy")}, {absence.Course.Name}\n");
        //}

        return stringBuilder.ToString();
    }
}
