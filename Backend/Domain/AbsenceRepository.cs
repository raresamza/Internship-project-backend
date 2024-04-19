using Backend.Application.Courses.Actions;
using Backend.Domain.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using Backend.Infrastructure.Contexts;
using Backend.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.Application.Abstractions;

public class AbsenceRepository : IAbsenceRepository
{

    private readonly AppDbContext _appDbContext;


    public AbsenceRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Absence CreateAbsence(Absence absence)
    {
        _appDbContext.Absences.Add(absence);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(CreateAbsence), true);
        return absence;
    }

    public void DeleteAbsence(Absence absence)
    {
        _appDbContext.Absences.Remove(absence);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(DeleteAbsence), true);
    }

    public Absence? GetByDateAndCourse(DateTime Date, Course course,Student student)
    {
        if (student.Absences == null)
        {
            throw new StudentAbsenceException($"The student: {student.Name} does not have any absences.");
        }

        var absence = _appDbContext.Absences.FirstOrDefault(a => a.Date == Date && a.Course.Name == course.Name);

        if(absence == null)
        {
            throw new StudentAbsenceException($"The student: {student.Name} does not have any absences on that day/course");
        }

        if (absence != null && student.Absences.Contains(absence))
            return absence;
        else
            throw new InvalidAbsenceException($"The specified absence does not exist.");
    }

    public Absence? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _appDbContext.Absences
            .Include(a => a.Course)
            .Include(a => a.Course.StudentCourses)
            .FirstOrDefault(a => a.Id == id);
    }

    //public int GetLastId()
    //{
    //    if (_absences.Count == 0) return 1;
    //    var lastId = _absences.Max(a => a.Id);
    //    return lastId + 1;
    //}

    public Absence UpdateAbsence(int absenceId, Absence newAbsence)
    {
        var oldAbsence = _appDbContext.Absences.FirstOrDefault(a => a.Id == absenceId);
        if (oldAbsence != null)
        {
            oldAbsence.Date = newAbsence.Date;
            oldAbsence.Course = newAbsence.Course;
            _appDbContext.Absences.Add(oldAbsence);
            _appDbContext.SaveChanges();
            return oldAbsence;
        }
        else
        {
            throw new InvalidAbsenceException($"The absence with id: {absenceId} was not found");
        }
    }
}
