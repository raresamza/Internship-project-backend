using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Response;

public class AbsenceDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }
    public Course? Course { get; set; }

    public static AbsenceDto FromAbsence(Absence absence)
    {
        return new AbsenceDto
        {
            Id = absence.Id,
            Course = absence.Course,
            Date = absence.Date,
        };
    }

    public override String ToString() 
    {
        return $"Absence(ID: {Id}) for the course \"{Course.Name}\"";
    }
}
