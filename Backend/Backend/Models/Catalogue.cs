using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;

namespace Backend.Models
{
    public class Catalogue
    {
        public Classroom Classroom { get; set; }
        public Catalogue(Classroom classroom)
        {
            Classroom = classroom;
        }

        public decimal computeGpa(Student student, Course course)
        {



            if (!course.Students.Contains(student))
            {
                StudentException.LogError();
                throw new StudentNotEnrolledException($"Student {student.Name} is not enrolled into the course {course.Name}");
            }
            decimal sum = 0;
            if (course == null)
            {
                throw new NullCourseException("This course is not valid.");
            }
            bool checkIfPresent = student.Grades.TryGetValue(course, out var list);
            if (list.Count == 0)
            {
                return 0;
            }
            if (checkIfPresent)
            {
                foreach (int grade in list)
                {
                    sum += grade;
                }
            }
            Message.GPAMessage(Math.Round(sum / list.Count(), 2), student, course.Name);
            student.addGpa(Math.Round(sum / list.Count(), 2), course);
            return Math.Round(sum / list.Count(), 2);
        }

        //public static void notifyParent(Student studen,Message message)
        //{
        //    Console.WriteLine("Notifying parent...");
        //}


    }
}
