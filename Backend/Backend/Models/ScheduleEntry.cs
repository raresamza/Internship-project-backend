using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class ScheduleEntry
{
    public TimeSlot TimeSlot { get; set; }
    public Course Course { get; set; }
    public Classroom Classroom { get; set; }
}


