using Backend.Application.Absences.Response;
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
    public Task<Absence> CreateAbsence(Absence absence);
    public Task<List<Absence>> GetAll(); 
    public Task<Absence?> GetByDateAndCourse(DateTime Date, Course course, Student student);
    public Task<Absence?> GetById(int id);

    public Task<Absence> UpdateAbsence(int absenceId, AbsenceUpdateDto newAbsence);
    public Task DeleteAbsence(Absence absence);

    //public void Add(Student s, int courseId);
}
