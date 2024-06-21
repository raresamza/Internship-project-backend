using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Backend.Infrastructure.Contexts;
using Backend.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
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

    public async Task<Catalogue> Create(Catalogue catalogue)
    {
        _appDbContext.Catalogues.Add(catalogue);
        await _appDbContext.SaveChangesAsync();
        return catalogue;
    }

    public async Task Delete(Catalogue catalogue)
    {
        _appDbContext.Catalogues.Remove(catalogue);
        await _appDbContext.SaveChangesAsync();
        await Logger.LogMethodCall(nameof(Delete), true);
    }

    public async Task<Catalogue?> GetById(int id)
    {
        await Logger.LogMethodCall(nameof(GetById), true);
        return await _appDbContext.Catalogues
            .FirstOrDefaultAsync(c => c.ID == id);
    }



    public async  Task<Catalogue> UpdateCatalogue(CatalogueUpdateDto catalogue, int id)
    {
        var oldCatalogue = await _appDbContext.Catalogues.FirstOrDefaultAsync(s => s.ID == id);
        if (oldCatalogue != null)
        {
            //oldCatalogue = catalogue;
            await _appDbContext.SaveChangesAsync();

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

    public void UndoGpa(Course course, Student student)
    {
        var studentGpa = student.GPAs.FirstOrDefault(g => g.CourseId == course.ID);
        if (studentGpa != null)
        {
            studentGpa.GPAValue = (decimal)0.00;

            _appDbContext.SaveChanges();

        }
        else
        {
            throw new StudentException($"Student {student.Name} is not enrolled into the course: {course.Name}, therefor the GPA cannot be computed");
        }
    }

    public async Task<List<Catalogue>> GetAll()
    {
        return await _appDbContext.Catalogues.ToListAsync();
    }
}
