using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions;

namespace Backend.Models
{
    public class Teacher : User
    {
        public Teacher(Subject subject, int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
        {
            Subject = subject;
        }
        public Subject Subject { get; set; }
        public Course TaughtCourse { get; set; }

        public void addToClassroom(Classroom classroom)
        {
            if (classroom == null)
            {
                throw new ClassroomException("This classroom is invalid");
            }
            if (classroom.Teachers.Contains(this))
            {
                throw new TeacherException($"Cannot assign {Name} teacher twice to the same class");
            }
            if (classroom.Teachers.Any(t => t.Subject == Subject))
            {
                throw new TeacherException($"A teacher teaching {Subject} is already assigned");
            }
            classroom.addTeacher(this);
        }


        public void AssignToCourse(Course course)
        {
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
            else
            {
                stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject} Taught Course: !Coruse is not assigned yet!\n");

            }
            return stringBuilder.ToString();
        }
    }
}
