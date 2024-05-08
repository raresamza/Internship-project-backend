using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Response;

public class ClassroomCreationDto
{
    public required int SchoolId { get; set; }
    public required string Name { get; set; }
}
