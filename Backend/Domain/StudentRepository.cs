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

    public void Delete(Student student)
    {
        _students.Remove(student);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Student UpdateStudent(Student student, int id)
    {
        var oldStudent = _students.FirstOrDefault(s => s.ID == id);
        if (oldStudent != null)
        {
            oldStudent = student;

            return oldStudent;
        }
        else
        {
            throw new StudentNotFoundException($"The student with id: {id} was not found");
        }
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

    public void RemoveGrade(Student student, Course course,int grade)
    {
        bool checkIfPresent = student.Grades.TryGetValue(course, out var list);
        if (checkIfPresent)
        {
            Logger.LogMethodCall(nameof(RemoveGrade), true);
            list.Remove(grade);
        }
        else
        {
            StudentException.LogError();
            Logger.LogMethodCall(nameof(RemoveGrade), false);
            throw new StudentException($"Student is not enrolled into the course {course.Name}");
        }
    }

    public void MotivateAbsence(DateTime date, Course course, Student student)
    {

        if (course == null)
        {
            CourseException.LogError();
            Logger.LogMethodCall(nameof(MotivateAbsence), false);
            throw new NullCourseException("This course is not valid");
        }
        else if (!course.Students.Contains(student))
        {
            StudentException.LogError();
            Logger.LogMethodCall(nameof(MotivateAbsence), false);
            throw new StudentNotEnrolledException($"Cannot motivate absence for {student.Name} because he is not enrolled into {course.Name}");
        }
        //foreach (Absence absence in student.Absences)
        //{
        //    if (absence.Date == date.Date && absence.Course.Name.Equals(course.Name))
        //    {
        //        student.Absences.Remove(absence);
        //        Logger.LogMethodCall(nameof(MotivateAbsence), true);

        //    }
        //}
        // Create a list to store absences to be removed
        List<Absence> absencesToRemove = new List<Absence>();

        // Iterate over the absences
        foreach (Absence absence in student.Absences.ToList())
        {
            if (absence.Date == date.Date && absence.Course.Name.Equals(course.Name))
            {
                // Add the absence to the list of items to be removed
                absencesToRemove.Add(absence);
                Logger.LogMethodCall(nameof(MotivateAbsence), true);
            }
        }

        // Remove the absences after the iteration
        foreach (Absence absenceToRemove in absencesToRemove)
        {
            student.Absences.Remove(absenceToRemove);
        }
    }

    public Student? GetByName(string name)
    {
        Logger.LogMethodCall(nameof(GetByName), true);
        return _students.FirstOrDefault(s => s.Name == name);
    }
}
