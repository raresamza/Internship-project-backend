using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Backend.Infrastructure.Contexts;
using Backend.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure;

public class CatalogueRepository : ICatalogueRepository
{
    private readonly AppDbContext _appDbContext;

    public CatalogueRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public double ComputeGpa(Student student, Course course)
    {
        if (student == null || course == null) 
        {
            throw new NullReferenceException("Student or course should not be null");
        }
        var studentGrade = student.Grades.FirstOrDefault(g => g.CourseId == course.ID);

        if (studentGrade != null)
        {
            var gradesForCourse = studentGrade.GradeValues;
            if (gradesForCourse.Any())
            {
                double averageGrade = gradesForCourse.Average();
                return averageGrade;
            }
            else
            {
                throw new InvalidOperationException($"No grades assigned for course: {course.Name}");
            }
        }
        else
        {
            throw new StudentNotEnrolledException($"Cannot assign a GPA for: {student.Name} because the student is not enrolled in the course: {course.Name}");
        }
    }

    public Catalogue Create(Catalogue catalogue)
    {
        _appDbContext.Catalogues.Add(catalogue);
        _appDbContext.SaveChanges();
        return catalogue;
    }

    public void Delete(Catalogue catalogue)
    {
        _appDbContext.Catalogues.Remove(catalogue);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Catalogue? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _appDbContext.Catalogues.FirstOrDefault(c => c.ID == id);
    }



    public Catalogue UpdateCatalogue(Catalogue catalogue, int id)
    {
        var oldCatalogue = _appDbContext.Catalogues.FirstOrDefault(s => s.ID == id);
        if (oldCatalogue != null)
        {
            oldCatalogue = catalogue;
            _appDbContext.SaveChanges();

            return oldCatalogue;

        }
        else
        {
            throw new NullClassroomException($"The classroom with id: {id} was not found");
        }
    }

    public void AddGpa(Course course, Student student)
    {
        var studentGpa = student.GPAs.FirstOrDefault(g => g.CourseId == course.ID);
        if (studentGpa != null)
        {
            studentGpa.GPAValue = (decimal)ComputeGpa(student, course);

            _appDbContext.SaveChanges();
            
        }
        else
        {
            throw new StudentException($"Student {student.Name} is not enrolled into the course: {course.Name}, therefor the GPA cannot be computed");
        }
    }

}
