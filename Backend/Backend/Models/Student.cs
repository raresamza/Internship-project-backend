using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;

namespace Backend.Domain.Models;
public class Student : User

//add school id to all classes and add to all methods check to see if from same school.
{
    //add dictionary of gpa's for final grade

    //public int classroomID { get; set; } = -1;

    //public Student(string parentEmail, string parentName, int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
    //{
    //    ID = ++_lastAssignedId;
    //    ParentName = parentName;
    //    ParentEmail = parentEmail;
    //}

    public Student() : base()
    {
        //ID = ++_lastAssignedId;
        Absences = _absences;
        Grades = _grades;
        GPAs = _gpas;
        StudentCoruses = _courses;
    }

    //private static int _lastAssignedId = 0;

    public int ID { get; set; }

    public bool Assigned { get; set; } = false;

    public Classroom Classroom { get; set; }

    public int? ClassroomId { get; set; }

    private readonly List<Absence> _absences = new();
    public ICollection<Absence> Absences { get; set; }

    public ICollection<StudentCourse> StudentCoruses { get; set; }

    private readonly List<StudentCourse> _courses = new();

    public required string ParentEmail { get; set; }

    public required string ParentName { get; set; }



    private readonly List<StudentGrade> _grades = new();
    private readonly List<StudentGPA> _gpas = new();
    public ICollection<StudentGrade> Grades { get; set; }
    public ICollection<StudentGPA> GPAs { get; set; }

  
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"\nStudent(ID: {ID}) details:\n\tStundent Name: {Name}\n\tStudent Age: {Age}\n\tStudent Phone Number: {PhoneNumber}\n\tStudent's Parent Name: {ParentName}\n\tStudent's Parent Email Addrees: {ParentEmail}\n\tStudent Address: {Address}\n\tStudent Grades:\n");

        foreach (var studentGrade in Grades)
        {
            Course course = studentGrade.Course;
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

        foreach (Absence absence in Absences)
        {
            stringBuilder.Append($"\t\t\t{absence.Date.ToString("dd/MM/yyyy")}, {absence.Course.Name}\n");
        }

        return stringBuilder.ToString();
    }


    //maybe add list of courses

}