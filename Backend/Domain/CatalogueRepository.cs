using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Backend.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure;

public class CatalogueRepository : ICatalogueRepository
{
    private List<Catalogue> _catalogues=new();

    public double ComputeGpa(Student student, Course course)
    {
        if(student == null)
        {
            throw new StudentNotFoundException($"Student with id: {student.ID} was not found");
        }
        if (course == null)
        {
            throw new NullCourseException($"Course with id: {course.ID} was not found");
        }

        if (student.Grades.ContainsKey(course))
        {
            List<int> gradesForCourse =student.Grades.First(pair => pair.Key.Equals(course)).Value;
            double averageGrade = gradesForCourse.Average();
            return averageGrade;
        }
        else
        {
            throw new StudentNotEnrolledException($"Cannot assign a GPA for: {student.Name} because the student is not enrolled in the course: {course.Name}");
        }
    }

    public Catalogue Create(Catalogue catalogue)
    {
        _catalogues.Add(catalogue);
        return catalogue;
    }

    public void Delete(Catalogue catalogue)
    {
        _catalogues.Remove(catalogue);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Catalogue? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _catalogues.FirstOrDefault(c => c.ID == id);
    }

    public int GetLastId()
    {
        if (_catalogues.Count == 0) return 1;
        var lastId = _catalogues.Max(a => a.ID);
        return lastId + 1;
    }

    public Catalogue UpdateCatalogue(Catalogue catalogue, int id)
    {
        var oldCatalogue = _catalogues.FirstOrDefault(s => s.ID == id);
        if (oldCatalogue != null)
        {
            oldCatalogue = catalogue;

            return oldCatalogue;
        }
        else
        {
            throw new NullClassroomException($"The classroom with id: {id} was not found");
        }
    }

    public void AddGpa(Course course, Student student)
    {
        if (student.GPAs.TryGetValue(course, out var GPA))
        {
            GPA = (decimal)ComputeGpa(student, course);
            student.GPAs[course] = GPA;

        }
        else
        {
            StudentException.LogError();
            throw new StudentException($"Student {student.Name} is not enrolled into the course: {course.Name}, therefor the GPA cannot be computed");
        }
    }

}
