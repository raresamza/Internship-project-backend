using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Students.Responses;

public class StudentCreationDto
{
    public required string ParentName { get; set; }
    public required string Name { get; set;}
    public required string ParentEmail {  get; set; }
    public required int PhoneNumber { get; set;}
    public required int Age { get; set; }
    public required string Address { get; set; }
}
