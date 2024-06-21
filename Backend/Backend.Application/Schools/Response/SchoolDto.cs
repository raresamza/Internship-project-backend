using Backend.Application.Classrooms.Response;
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
    public ICollection<ClassroomDto> Classrooms { get; set; }

    public static SchoolDto FromScool(UpdateSchoolDto school) 
    {
        return new SchoolDto
        {
            Name = school.Name,
            ID = school.ID,
            Classrooms = school.Classrooms.Select((classroom) => ClassroomDto.FromClassroom(classroom)).ToList(),
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"The school with name: \"{Name}\" has the following classrooms:\n");
        foreach(ClassroomDto c in  Classrooms)
        {
            sb.Append("\t"+ c.ToString()+"\n");
        }
        return sb.ToString();

    }
}
