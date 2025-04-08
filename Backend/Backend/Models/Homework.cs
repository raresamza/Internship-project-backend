using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class Homework
{


    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public float? Grade { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public ICollection<StudentHomework> StudentHomeworks { get; set; } = new List<StudentHomework>();

    //private readonly List<StudentHomework> studentHomeworks = new();

    //public

}
