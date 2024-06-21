using Backend.Application.Absences.Response;
using Backend.Application.Courses.Actions;
using Backend.Domain.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using Backend.Infrastructure.Contexts;
using Backend.Infrastructure.Utils;
using Castle.Core.Logging;
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

    public async Task<Absence> CreateAbsence(Absence absence)
    {
        _appDbContext.Absences.Add(absence);
        await _appDbContext.SaveChangesAsync();
        await Logger.LogMethodCall(nameof(CreateAbsence), true);
        return absence;
    }

    public async Task DeleteAbsence(Absence absence)
    {
        _appDbContext.Absences.Remove(absence);
        await _appDbContext.SaveChangesAsync();
        await Logger.LogMethodCall(nameof(DeleteAbsence), true);
    }

    public async Task<List<Absence>> GetAll()
    {
        return await _appDbContext.Absences
            .Include(a => a.Course)
            .Include(a => a.Course.StudentCourses)
            .ToListAsync();
    }

    public async Task<Absence?> GetByDateAndCourse(DateTime Date, Course course,Student student)
    {
        if (student.Absences == null)
        {
            throw new StudentAbsenceException($"The student: {student.Name} does not have any absences.");
        }

        var absence = await _appDbContext.Absences.FirstOrDefaultAsync(a => a.Date == Date && a.Course.Name == course.Name);

        if(absence == null)
        {
            throw new StudentAbsenceException($"The student: {student.Name} does not have any absences on that day/course");
        }

        if (absence != null && student.Absences.Contains(absence))
            return absence;
        else
            throw new InvalidAbsenceException($"The specified absence does not exist.");
    }

    public async Task<Absence?> GetById(int id)
    {
        await Logger.LogMethodCall(nameof(GetById), true);
        return await _appDbContext.Absences
            .Include(a => a.Course)
            .Include(a => a.Course.StudentCourses)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Absence> UpdateAbsence(int absenceId, AbsenceUpdateDto newAbsence)
    {
        var oldAbsence = await _appDbContext.Absences.FirstOrDefaultAsync(a => a.Id == absenceId);
        if (oldAbsence != null)
        {
            oldAbsence.Date = newAbsence.Date;
            //oldAbsence.Course = newAbsence.Course;
            _appDbContext.Absences.Add(oldAbsence);
            await _appDbContext.SaveChangesAsync();
            return oldAbsence;
        }
        else
        {
            throw new InvalidAbsenceException($"The absence with id: {absenceId} was not found");
        }
    }
}
