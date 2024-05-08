using Backend.Application.Courses.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Response;

public class AbsenceDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }
    [JsonIgnore]

    public Course? Course { get; set; }
    public String? CourseName { get; set; }
    //public CourseDto? Course { get; set; }

    public static AbsenceDto FromAbsence(Absence absence)
    {
        return new AbsenceDto
        {
            Id = absence.Id,
            CourseName = absence.Course?.Name,
            //Course = CourseDto.FromCourse(absence.Course),
            Date = DateTime.TryParseExact(absence.Date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : default(DateTime),
            //Date = absence.Date.,
        };
    }

    public override String ToString() 
    {
        return $"Absence(ID: {Id}) for the course \"{Course.Name}\"";
    }
}
