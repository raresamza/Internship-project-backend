using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;

namespace Backend.Application.Homeworks.Response;

public class StudentHomeworkDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int HomeworkId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public float? Grade { get; set; }

    // Optional: For display purposes
    public string? StudentName { get; set; }
    public string? HomeworkTitle { get; set; }


    public static StudentHomeworkDto FromStudentHomework(StudentHomework studentHomework)
    {
        return new StudentHomeworkDto
        {
            Id = studentHomework.ID,
            StudentId = studentHomework.StudentId,
            HomeworkId = studentHomework.HomeworkId,
            HomeworkTitle = studentHomework.Homework.Title,
            IsCompleted = studentHomework.IsCompleted,
            SubmissionDate = studentHomework.SubmissionDate,
            Grade = studentHomework.Grade,
            StudentName = studentHomework.Student.Name
        };
    }
}