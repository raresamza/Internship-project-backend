using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;
public class Message
{
    public static void GradeMessage(int grade, Student student, string courseName)
    {
        Console.WriteLine($"Student {student.Name} recieved {grade} in {courseName}. Parent {student.ParentName} was notified at {student.ParentEmail}");
    }

    public static void GPAMessage(decimal grade, Student student, string courseName)
    {
        Console.WriteLine($"Student {student.Name} finished course {courseName} with grade: {grade}. Parent {student.ParentName} was notified at {student.ParentEmail}");
    }

    public static void AbsenceMessage(Student student, Absence absence)
    {
        Console.WriteLine($"Student {student.Name} was marked absent in {absence.Course.Name} on {absence.Date.ToString("dd/MM/yyyy")}. Parent {student.ParentName} was notified at {student.ParentEmail}");
    }
}
