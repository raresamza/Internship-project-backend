using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public interface IAbsenceRepository
{
    public Absence CreateAbsence(Absence absence);
    public Absence? GetByDateAndCourse(DateTime Date, Course course, Student student);
    //public int GetLastId();
    public Absence? GetById(int id);

    public Absence UpdateAbsence(int absenceId, Absence newAbsence);
    public void DeleteAbsence(Absence absence);

    //public void Add(Student s, int courseId);
}
