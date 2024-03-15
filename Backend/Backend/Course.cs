using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Course
    {

        public Course(string name, Subject subject)
        {
            Name = name;
            Subject = subject;
        }

        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Teacher != null)
            {
                sb.Append($"The course {Name} has the teacher {Teacher.Name} and {Students.Count} students and the list of students enrolled is:\n");

            }
            else 
            {
                sb.Append($"The course \"{Name}\" is currently uninitilized, please proceed to do so.");
                return sb.ToString();
            }
            foreach (Student student in Students)
            {
                sb.Append($"\t\t{student.Name}");
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
