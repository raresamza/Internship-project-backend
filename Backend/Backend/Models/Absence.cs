using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Models;

public class Absence
{

    public DateTime Date { get; set; } = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
    public Course Course { get; set; }

    public int Id { get; set; }

    //public Absence(Course course)
    //{
    //    Course = course;
    //}

    //public Absence(Course course)
    //{
    //}

    public Absence()
    {
    }

    public Absence GetAbsenceByDate(DateTime date)
    {
        //dummy method waiting for db
        return null;
    }

    
}