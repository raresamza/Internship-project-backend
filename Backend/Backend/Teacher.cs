using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Teacher:User
    {
        public Teacher(Subject subject,int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
        {
            Subject = subject;
        }

        public void addToClassroom(Classroom classroom)
        {
            classroom.addTeacher(this);
        }

        public Subject Subject { get; set; }
        public Course TaughtCourse { get; set; }
        public void AssignToCourse(Course course) {
            if (Subject == course.Subject)
            {
                course.Teacher = this;
                TaughtCourse = course;
            }
            else 
            {
                TeacherException.LogError();
                throw new TeacherException($"The subject that the teacher spelcializes in: {Subject} does not match with the course subject: {course.Subject}");
            }
            
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (TaughtCourse != null)
            {
                stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject}\n\tTaught Course: {TaughtCourse.ToString()}");

            }
            else {
                stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject} Taught Course: !Coruse is not assigned yet!\n");

            }
            return stringBuilder.ToString();
        }
    }
}
