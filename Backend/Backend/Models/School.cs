using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.Placeholders;

namespace Backend.Domain.Models;
public class School
{
    public int ID { get; set; }
    public required string  Name { get; set; }

    public List<Classroom> _classrooms = new();
    public ICollection<Classroom> Classrooms { get; set; } 
    public School(string name)
    {
        Name = name;
    }
    public School() 
    { 
        Classrooms=_classrooms;
    }


}
