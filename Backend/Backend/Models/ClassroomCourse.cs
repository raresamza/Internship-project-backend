using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class ClassroomCourse
{
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}

