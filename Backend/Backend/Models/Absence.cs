using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class Absence
{

    public DateTime Date { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public int? StudentId { get; set; }

    public Student? Student { get; set; }

    public int Id { get; set; }

    //public Absence(Course course)
    //{
    //    Course = course;
    //}

    //public Absence(Course course)
    //{
    //}

    public Absence(DateTime date)
    {
        Date = date;
    }

    public Absence GetAbsenceByDate(DateTime date)
    {
        //dummy method waiting for db
        return null;
    }

    
}