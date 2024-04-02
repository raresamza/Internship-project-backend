using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Backend.Application.Abstractions;
using Backend.Exceptions.TeacherException;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Application.Courses.Actions;
using Backend.Exceptions.Placeholders;
using Backend.Infrastructure.Utils;
using System.Xml.Linq;

namespace Backend.Infrastructure;

public class StudentRepository : IStudentRepository
{

    //add private field list of students

    private readonly List<Student> _students = new();

    public List<Student> GetAllStudents()
    {
        return _students;
    }

    //db mock

    public Student? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _students.FirstOrDefault(s => s.ID == id);
    }

    public Student Create(Student student)
    {
        _students.Add(student);
        Logger.LogMethodCall(nameof(Create), true);
        return student;
    }

    public int GetLastId()
    {
        if (_students.Count == 0) return 1;
        var lastId = _students.Max(a => a.ID);
        return lastId + 1;
    }

    public void Delete(int id)
    {
        var student = _students.FirstOrDefault(s => s.ID == id);
        _students.Remove(student);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public void UpdateStudent(Student student, int id)
    {
        var oldStudent = GetById(id);
        if (oldStudent == null)
        {
            throw new TeacherNotFoundException($"Teacher with id {id} not found");
        }
        oldStudent = student;
    }

    public void AddGrade(int grade, Student student, Course course)
    {
        

        bool checkIfPresent = student.Grades.TryGetValue(course, out var list);
        if (checkIfPresent)
        {
            Message.GradeMessage(grade, student, course.Name);
            Logger.LogMethodCall(nameof(AddGrade), true);
            list.Add(grade);
        }
        else
        {
            StudentException.LogError();
            Logger.LogMethodCall(nameof(AddGrade), false);
            throw new StudentException($"Student {student.Name} is not enrolled into the course: {course.Name}, therefor he can not be assigned a grade for this course");
        }
    }

    public void EnrollIntoCourse(Student student, Course course)
    {
        List<int> grades = new List<int>();
        course.Students.Add(student);
        student.GPAs.Add(course, 0);
        student.Grades.Add(course, grades);
        Logger.LogMethodCall(nameof(EnrollIntoCourse), true);
    }

    public void AddAbsence(Student student, Absence absence)
    {
        

        if (!absence.Course.Students.Contains(student))
        {
            AbsenceException.LogError();
            Logger.LogMethodCall(nameof(AddAbsence), false);
            throw new InvalidAbsenceException($"Cannot mark student {student.Name} as absent in \"{absence.Course.Name}\" because student is not enrolled in it");
        }
        else if (student.Absences.Any(d => d.Date == absence.Date && d.Course.Subject == absence.Course.Subject))
        {
            Logger.LogMethodCall(nameof(AddAbsence), false);
            throw new DuplicateAbsenceException($"Cannot mark student {student.Name} as absent twice in the same day ({absence.Date.ToString("dd/MM/yyyy")}) for the same course ({absence.Course.Name})");
        }
        Logger.LogMethodCall(nameof(AddAbsence), true);
        student.Absences.Add(absence);
    }

    
}
