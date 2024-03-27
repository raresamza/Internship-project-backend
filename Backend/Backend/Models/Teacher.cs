using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.TeacherException;

namespace Backend.Models
{
    public class Teacher : User
    {
        public Teacher(Subject subject, int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
        {
            ID = ++_lastAssignedId;
            Subject = subject;
        }
        public Subject Subject { get; set; }
        public Course TaughtCourse { get; set; }

        private static int _lastAssignedId = 0;

        public int ID { get; }

        //public void AddToClassroom(Classroom classroom)
        //{
        //    if (classroom == null)
        //    {
        //        throw new NullClassroomException("This classroom is invalid");
        //    }
        //    if (classroom.Teachers.Contains(this))
        //    {
        //        throw new TeacherAlreadyAssignedException($"Cannot assign {Name} teacher twice to the same class");
        //    }
        //    if (classroom.Teachers.Any(t => t.Subject == Subject))
        //    {
        //        throw new TeacherSubjectMismatchException($"A teacher teaching {Subject} is already assigned");
        //    }
        //    classroom.AddTeacher(this);
        //}


        //public void AssignToCourse(Course course)
        //{
        //    if (Subject == course.Subject)
        //    {
        //        course.Teacher = this;
        //        TaughtCourse = course;
        //    }
        //    else
        //    {
        //        TeacherException.LogError();
        //        throw new TeacherSubjectMismatchException($"The subject that the teacher spelcializes in: {Subject} does not match with the course subject: {course.Subject}");
        //    }

        //}

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
