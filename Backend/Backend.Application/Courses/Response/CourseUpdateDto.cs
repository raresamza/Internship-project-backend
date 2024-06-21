using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Response;

public class CourseUpdateDto
{
    public required string Name { get; set; }
    public required Subject Subject { get; set; }

}
