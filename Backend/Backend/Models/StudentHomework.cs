using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class StudentHomework
{
    public int ID { get; set; }
    public int StudentId { get; set; }
    public int HomeworkId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public float? Grade { get; set; }

    public Student Student { get; set; } = null!;
    public Homework Homework { get; set; } = null!;

    public string? FileUrl { get; set; }

}
