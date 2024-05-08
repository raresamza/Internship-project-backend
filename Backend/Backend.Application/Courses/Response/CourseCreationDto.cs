using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Courses.Response;

public class CourseCreationDto
{
    public string Name { get; set; }
    public Subject Subject { get; set; }

    public static CourseCreationDto FromCourse(Course course)
    {
        return new CourseCreationDto
        {
            Subject = course.Subject,
            Name = course.Name,
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Name: {Name}\nSubject: {Subject}\n");
        return sb.ToString();
    }
}
