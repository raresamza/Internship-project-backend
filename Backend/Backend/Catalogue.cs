using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Catalogue
    {
        public Classroom Classroom {  get; set; }
        public Catalogue (Classroom classroom)
        {
            Classroom = classroom;
        }

        public decimal computeGpa(Student student, Course course)
        { 
            if(!course.Students.Contains(student))
            {
                StudentException.LogError();
                throw new StudentException($"Student {student.Name} is not enrolled into the course {course.Name}");
            }
            decimal sum = 0;
            if(course == null)
            {
                throw new CourseException("This course is not valid.");
            }
            bool checkIfPresent = student.Grades.TryGetValue(course, out var list);
            if(list.Count == 0)
            {
                return 0;
            }
            if(checkIfPresent)
            {
                foreach(int grade in list)
                {
                    sum += grade;
                }
            }
            notifyParent(student);
            student.addGpa(Math.Round(sum / list.Count(), 2), course);
            Console.WriteLine();
            return Math.Round(sum / list.Count(),2);
        }


        public void notifyParent(Student student)
        {
            Console.WriteLine("Notifying parent...");
        }

        
    }
}
