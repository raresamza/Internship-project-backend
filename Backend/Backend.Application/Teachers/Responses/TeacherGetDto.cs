using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Responses;

public class TeacherGetDto
{
    public int ID { get; set; }
    public required int Age { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required int PhoneNumber { get; set; }
    public required Subject Subject { get; set; }



    public static TeacherGetDto FromTeacher(Teacher teacher)
    {
        return new TeacherGetDto
        {

            Address = teacher.Address,
            Age = teacher.Age,
            Name = teacher.Name,
            PhoneNumber = teacher.PhoneNumber,
            Subject = teacher.Subject,
            ID = teacher.ID,
        };
    }
}
