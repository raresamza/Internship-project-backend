using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;

namespace Backend.Domain.Models;

public class Catalogue


{
    //make sure to assign catalogue to classroom;
    public Classroom Classroom { get; set; }
    public int ClassroomId { get; set; }
    public int ID { get; set; }
    public Catalogue(Classroom classroom)
    {
        Classroom = classroom;
    }

    public Catalogue()
    {

    }

    public decimal ComputeGpa(Student student, Course course)
    {



        if (!course.StudentCourses.Any(sc => sc.Student == student))
        {
            StudentException.LogError();
            throw new StudentNotEnrolledException($"Student {student.Name} is not enrolled into the course {course.Name}");
        }
        decimal sum = 0;
        if (course == null)
        {
            throw new NullCourseException("This course is not valid.");
        }
        var studentGrade = student.Grades.FirstOrDefault(g => g.Course == course);
        if (studentGrade == null || studentGrade.GradeValues.Count == 0)
        {
            return 0;
        }

        foreach (int grade in studentGrade.GradeValues)
        {
            sum += grade;
        }
        Message.GPAMessage(Math.Round(sum / studentGrade.GradeValues.Count(), 2), student, course.Name);
        return Math.Round(sum / studentGrade.GradeValues.Count(), 2);
    }

    //public static void notifyParent(Student studen,Message message)
    //{
    //    Console.WriteLine("Notifying parent...");
    //}


}
