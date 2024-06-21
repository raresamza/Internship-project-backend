using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class TeacherClassroom
{
    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }

    //public Subject Subject { get; set; }

}
