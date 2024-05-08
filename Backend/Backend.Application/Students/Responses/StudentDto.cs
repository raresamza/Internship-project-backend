using Backend.Application.Absences.Response;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
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
    public ICollection<StudentGradeDto>? Grades { get; set; }
    //[JsonIgnore]
    //public ICollection<StudentGrade> Grades { get; set; }
    //[JsonIgnore]
    //public ICollection<StudentGPA> GPAs { get; set; }
    public ICollection<StudentGPADto>? GPAs { get; set; }
    //[JsonIgnore]
    public ICollection<AbsenceDto>? Absences { get; set; }


    //public ICollection<AbsenceDto> Absences { get; set; }

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
            Grades = student.Grades.Select((studentGrade) => StudentGradeDto.FromStudentGrade(studentGrade)).ToList(),
            GPAs = student.GPAs.Select((studentGpa) => StudentGPADto.FromStudentGpa(studentGpa)).ToList(),
            Absences = student.Absences.Select((absence) => AbsenceDto.FromAbsence(absence)).ToList(),
            //Grades = student.Grades,
            //Absences = student.Absences,
            //GPAs = student.GPAs
        };
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"\nStudent(ID: {ID}) details:\n\tStundent Name: {Name}\n\tStudent Age: {Age}\n\tStudent Phone Number: {PhoneNumber}\n\tStudent's Parent Name: {ParentName}\n\tStudent's Parent Email Addrees: {ParentEmail}\n\tStudent Address: {Address}\n\tStudent Grades:\n");

        foreach (var studentGrade in Grades)
        {
            CourseDto course = studentGrade.Course;
            Console.WriteLine(studentGrade.ToString());
            List<int> grades = studentGrade.GradeValues;
            stringBuilder.Append($"\t\tCourse: {course.Name}\n");

            stringBuilder.Append("\t\t\tGrades: ");
            foreach (var grade in grades)
            {
                stringBuilder.Append($"{grade} ");
            }
            stringBuilder.Append("\n\t\t\tGPA: ");

            // Find GPA for the current course
            var studentGPA = GPAs.FirstOrDefault(g => g.Course == course);
            if (studentGPA != null)
            {
                stringBuilder.Append(studentGPA.GPAValue.ToString());
            }
            else
            {
                stringBuilder.Append("N/A");
            }
            stringBuilder.Append('\n');
        }

        stringBuilder.Append($"\t\tAbsences:\n");

        foreach (AbsenceDto absence in Absences)
        {
            stringBuilder.Append($"\t\t\t{absence.Date.ToString("dd/MM/yyyy")}, {absence.Course.Name}\n");
        }

        return stringBuilder.ToString();
    }

}
