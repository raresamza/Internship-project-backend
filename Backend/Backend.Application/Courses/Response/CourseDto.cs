using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Response;

public class CourseDto
{

    public required string Name { get; set; }
    public required Subject Subject { get; set; }

    public int ID { get; set; }

    public static CourseDto FromCourse(Course course)
    {
        return new CourseDto
        {
            ID = course.ID,
            Subject= course.Subject,
            Name = course.Name,

        };
    }
}
