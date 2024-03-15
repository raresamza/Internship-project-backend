using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Student : User
    {

        public bool Assigned {  get; set; }=false;
        public Student(string parentEmail, string parentName, int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
        {
            ParentName = parentName;
            ParentEmail = parentEmail;
        }

        public string ParentEmail { get; set; }

        public string ParentName { get; set; }
        public Dictionary<Course, List<int>> Grades { get; set; } = new Dictionary<Course, List<int>>();

        public void addToClassroom(Classroom classroom) {
            //check if already in other classrooms;
            
        }

        //public void enrollStudnet(Course course) {
        //    List<int> grades = new List<int>();
        //    Grades.Add(course, grades);
        //}
        public void addGrade(int grade, Course course)
        {
            
            bool checkIfPresent = Grades.TryGetValue(course, out var list);
            if (checkIfPresent)
            {
                list.Add(grade);
            }
            else
            {
                StudentException.LogError();
                throw new StudentException($"Student {Name} is not enrolled into the course: {course.Name}, therefor he can not be assigned a grade for this course");
            }
        }

        public void removeGrade(int grade, Course course)
        {
            bool checkIfPresent = Grades.TryGetValue(course, out var list);
            if (checkIfPresent)
            {
                list.Remove(grade);
            }
            else
            {
                StudentException.LogError();
                throw new StudentException($"Student is not enrolled into the course {course.Name}");
            }
        }
        public void enrollIntoCourse(Course course)
        {
            if(course.Students.Contains(this))
            {
                StudentException.LogError();
                throw new StudentException($"Student {Name} is already enrolled into this course");
            }
            course.Students.Add(this);
            List<int> grades = new List<int>();
            Grades.Add(course, grades);
            
            //Console.WriteLine(course.Students);
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"\nStudent details:\n\tStundent Name: {Name}\n\tStudent Age: {Age}\n\tStudent Phone Number: {PhoneNumber}\n\tStudent's Parent Name: {ParentName}\n\tStudent's Parent Email Addrees: {ParentEmail}\n\tStudent Address: {Address}\n\tStudent Grades:\n");
            foreach (var entry in Grades)
            {
                Course course = entry.Key;
                List<int> grades = entry.Value;
                stringBuilder.Append($"\t\tCourse: {course.Name}\n");
                
                stringBuilder.Append("\t\t\tGrades: ");
                foreach (var grade in grades)
                {
                    stringBuilder.Append($"{grade} ");
                }
                stringBuilder.Append('\n');

            }
            return stringBuilder.ToString();
        }

        //maybe add list of courses

    }
}
