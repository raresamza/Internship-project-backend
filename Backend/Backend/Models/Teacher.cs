using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.TeacherException;

namespace Backend.Domain.Models;

public class Teacher : User
{
    public Teacher(Subject subject, int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
    {
        //ID = ++_lastAssignedId;
        Subject = subject;
    }
    public Teacher()
    {

    }
    [Required]
    public required Subject Subject { get; set; }
    public int? TaughtCourseId { get; set; }

    public Course TaughtCourse { get; set; }
    public ICollection<TeacherClassroom> TeacherClassrooms { get; set; }

    public int ID { get; set; }
    public override string ToString()
    {

        StringBuilder stringBuilder = new StringBuilder();
        if (TaughtCourse != null)
        {
            stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject}\n\tTaught Course: {TaughtCourse.ToString()}\n");

        }
        else
        {
            stringBuilder.Append($"Teacher details:\n\tTeacher Name: {Name}\n\tTeacher Age: {Age}\n\tTeacher Phone Number: {PhoneNumber}\n\tTeacher Address: {Address}\n\tTeacher Subject: {Subject}\n\tTaught Course: !Coruse is not assigned yet!\n");

        }
        return stringBuilder.ToString();
    }
}