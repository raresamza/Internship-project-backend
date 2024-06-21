using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Teachers.Responses;

public class TeacherUpdateDto
{

    public required int Age { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required int PhoneNumber { get; set; }
    public required Subject Subject { get; set; }

    //public string? CourseName { get; set; }
}
