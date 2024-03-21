using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
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
        public void addClassroom(Classroom c)
        {
            if (c == null)
            {
                ClassroomException.LogError();
                throw new ClassroomException($"Classroom is not valid");
            }
            else if (Classrooms.Contains(c))
            {
                ClassroomException.LogError();
                throw new ClassroomException($"Classroom {c.Name} is already registered");
            }
            Classrooms.Add(c);
        }
        public void removeClassroom(Classroom c)
        {
            if (c == null)
            {
                ClassroomException.LogError();
                throw new ClassroomException("Classroom is not valid");
            }
            else if (!Classrooms.Contains(c))
            {
                ClassroomException.LogError();
                throw new ClassroomException($"Classroom {c.Name} cannot be deleted because it is not registered");
            }
            Classrooms.Remove(c);
        }

    }
}
