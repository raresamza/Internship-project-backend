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
    int ID { get; set; }
    string Name { get; set; }
    List<Classroom> Classrooms { get; set; } = new List<Classroom>();
    public School(int ID, string name)
    {
        this.ID = ID;
        Name = name;
    }
    public void AddClassroom(Classroom c)
    {
        if (c == null)
        {
            ClassroomException.LogError();
            throw new NullClassroomException($"Classroom is not valid");
        }
        else if (Classrooms.Contains(c))
        {
            ClassroomException.LogError();
            throw new ClassroomAlreadyRegisteredException($"Classroom {c.Name} is already registered");
        }
        Classrooms.Add(c);
    }
    public void RemoveClassroom(Classroom c)
    {
        if (c == null)
        {
            ClassroomException.LogError();
            throw new NullClassroomException("Classroom is not valid");
        }
        else if (!Classrooms.Contains(c))
        {
            ClassroomException.LogError();
            throw new ClassroomNotRegisteredException($"Classroom {c.Name} cannot be deleted because it is not registered");
        }
        Classrooms.Remove(c);
    }

}
