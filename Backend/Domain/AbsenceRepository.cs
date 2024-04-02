using Backend.Application.Courses.Actions;
using Backend.Domain.Models;
using Backend.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public class AbsenceRepository : IAbsenceRepository
{

    private readonly List<Absence> _absences = new();

    public Absence CreateAbsence(Absence absence)
    {
        _absences.Add(absence);
        Logger.LogMethodCall(nameof(CreateAbsence), true);
        return absence;
    }

    public void DeleteAbsence(int id)
    {
        var absence = _absences.FirstOrDefault(a => a.Id == id);
        _absences.Remove(absence);
        Logger.LogMethodCall(nameof(DeleteAbsence), true);
    }

    public Absence GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _absences.FirstOrDefault(a => a.Id == id);
    }

    public int GetLastId()
    {
        if (_absences.Count == 0) return 1;
        var lastId = _absences.Max(a => a.Id);
        return lastId + 1;
    }
}
