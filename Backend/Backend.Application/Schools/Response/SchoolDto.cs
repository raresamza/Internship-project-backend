using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Schools.Response;

public class SchoolDto
{
    public int ID { get; set; }
    public required string Name { get; set; }
    public List<Classroom> classrooms = new();
    public ICollection<Classroom> Classrooms { get; set; }

    public static SchoolDto FromScool(School school) 
    {
        return new SchoolDto
        {
            Name = school.Name,
            ID = school.ID,
            Classrooms = school.Classrooms,
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"The school with name: \"{Name}\" has the following classrooms:\n");
        foreach(Classroom c in  Classrooms)
        {
            sb.Append("\t"+ c.ToString()+"\n");
        }
        return sb.ToString();

    }
}
