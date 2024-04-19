using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Backend.Application.Abstractions;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Infrastructure.Utils;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Students.Update;

namespace Backend.Infrastructure;

public class StudentRepository : IStudentRepository
{

    //add private field list of students

    private readonly AppDbContext _appDbContext;
    //private readonly List<Student> _students = new();

    public StudentRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public List<Student> GetAllStudents()
    {
        return _appDbContext.Students.ToList();
    }

    //db mock

    public Student? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        //return _students.FirstOrDefault(s => s.ID == id);
        return _appDbContext.Students
            .Include(s => s.Absences)
            .Include(s => s.Grades)
                .ThenInclude(sg=> sg.Course)
            .Include(s => s.GPAs)
            .FirstOrDefault(s => s.ID == id);
    }

    public Student Create(Student student)
    {
        _appDbContext.Students.Add(student);
        _appDbContext.SaveChanges();
        //_students.Add(student);
        Logger.LogMethodCall(nameof(Create), true);

        return student;
    }

    //public int GetLastId()
    //{
    //    if (_students.Count == 0) return 1;
    //    var lastId = _students.Max(a => a.ID);
    //    return lastId + 1;
    //}

    public void Delete(Student student)
    {
        _appDbContext.Students.Remove(student);
        _appDbContext.SaveChanges();
        //_students.Remove(student);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Student UpdateStudent(Student student, int id)
    {
        var existingStudent = _appDbContext.Students.Find(id);

        if (existingStudent != null)
        {
            existingStudent.Assigned = student.Assigned;
            existingStudent.Classroom = student.Classroom;
            existingStudent.Absences = student.Absences;
            existingStudent.StudentCoruses = student.StudentCoruses;
            existingStudent.ParentEmail = student.ParentEmail;
            existingStudent.ParentName = student.ParentName;
            existingStudent.Grades = student.Grades;
            existingStudent.GPAs = student.GPAs;

            _appDbContext.SaveChanges();

            return existingStudent;
        }
        else
        {
            throw new StudentNotFoundException($"The student with id: {id} was not found");
        }
    }

    public void AddGrade(int grade, Student student, Course course)
    {

        if (student != null)
        {
            var studentGrade = student.Grades.FirstOrDefault(g => g.Course.Name== course.Name);

            if (studentGrade != null)
            {
                studentGrade.GradeValues.Add(grade);
                studentGrade.Course = course;
                Logger.LogMethodCall(nameof(AddGrade), true);
                //_appDbContext.StudentGrades.Add(studentGrade);
                _appDbContext.SaveChanges();
            }
            else
            {
                Logger.LogMethodCall(nameof(AddGrade), false);
                throw new StudentException($"Student {student.Name} is not enrolled in the course: {course.Name}, therefore they cannot be assigned a grade for this course");
            }
        }
        else
        {
            throw new StudentNotFoundException($"Student with ID: {student.ID} not found");
        }
    }

    public void EnrollIntoCourse(Student student, Course course)
    {
        List<int> grades = new List<int>();
        var studentCourse = new StudentCourse 
        { 
            Student = student, 
            Course = course, 
            StudentId = student.ID, 
            CourseId = course.ID 
        };
        var studentGpa = new StudentGPA
        {
            Student = student,
            StudentId = student.ID,
            CourseId = course.ID,
            Course = course,
            GPAValue = 0
        };
        var studentGrade = new StudentGrade
        {
            Course = course,
            StudentId = student.ID,
            Student = student,
            CourseId = course.ID,
            GradeValues = grades
        };

        course.StudentCourses.Add(studentCourse);
        student.GPAs.Add(studentGpa);
        student.Grades.Add(studentGrade);


        _appDbContext.StudentCourses.Add(studentCourse);
        _appDbContext.StudentGPAs.Add(studentGpa);
        _appDbContext.StudentGrades.Add(studentGrade);

        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(EnrollIntoCourse), true);
    }

    public void AddAbsence(Student student, Absence absence)
    {


        if (!absence.Course.StudentCourses.Any(sc => sc.Student.ID == student.ID))
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
        _appDbContext.SaveChanges();
    }

    public void RemoveGrade(Student student, Course course, int grade)
    {
        var studentGrade = student.Grades.FirstOrDefault(g => g.CourseId == course.ID);
        if (studentGrade != null)
        {
            studentGrade.GradeValues.Remove(grade);
            _appDbContext.SaveChanges();
            Logger.LogMethodCall(nameof(RemoveGrade), true);
        }
        else
        {
            StudentException.LogError();
            Logger.LogMethodCall(nameof(RemoveGrade), false);
            throw new StudentException($"Student is not enrolled into the course {course.Name}");
        }

        _appDbContext.SaveChanges();
    }

    public void MotivateAbsence(DateTime date, Course course, Student student)
    {

        if (course == null)
        {
            CourseException.LogError();
            Logger.LogMethodCall(nameof(MotivateAbsence), false);
            throw new NullCourseException("This course is not valid");
        }
        else if (!course.StudentCourses.Any(sc => sc.Student == student))
        {
            StudentException.LogError();
            Logger.LogMethodCall(nameof(MotivateAbsence), false);
            throw new StudentNotEnrolledException($"Cannot motivate absence for {student.Name} because he is not enrolled into {course.Name}");
        }
        List<Absence> absencesToRemove = new List<Absence>();

        foreach (Absence absence in student.Absences.ToList())
        {
            if (absence.Date == date.Date && absence.Course.Name.Equals(course.Name))
            {
                absencesToRemove.Add(absence);
                Logger.LogMethodCall(nameof(MotivateAbsence), true);
            }
        }

        foreach (Absence absenceToRemove in absencesToRemove)
        {
            student.Absences.Remove(absenceToRemove);
            _appDbContext.Absences.Remove(absenceToRemove);
        }

        _appDbContext.SaveChanges();
    }

    public Student? GetByName(string name)
    {
        Logger.LogMethodCall(nameof(GetByName), true);
        return _appDbContext.Students.FirstOrDefault(s => s.Name == name);
    }
}
