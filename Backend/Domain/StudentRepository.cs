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
using Backend.Application.Students.Responses;
using Backend.Application.Leaderboard.Responses;

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

    public async Task<Student?> GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        //return _students.FirstOrDefault(s => s.ID == id);
        return await _appDbContext.Students
            .Include(s => s.Absences)
            .Include(s => s.Grades)
                .ThenInclude(sg=> sg.Course)
            .Include(s => s.GPAs)
            .Include(s => s.StudentCoruses)
                .ThenInclude(sc => sc.Course)
            .FirstOrDefaultAsync(s => s.ID == id);
    }

    public async Task<Student> Create(Student student)
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

    public async Task Delete(Student student)
    {
        _appDbContext.Students.Remove(student);
        await _appDbContext.SaveChangesAsync();
        //_students.Remove(student);
        Logger.LogMethodCall(nameof(Delete), true);
    }
    public async Task<Student> UpdateStudent(StudentUpdateDto student, int id)
    {
        var existingStudent =await _appDbContext.Students.FindAsync(id);

        if (existingStudent != null)
        {
            //existingStudent.Assigned = student.Assigned;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.Address= student.Address;
            existingStudent.ParentEmail = student.ParentEmail;
            existingStudent.ParentName = student.ParentName;

            _appDbContext.SaveChanges();

            return existingStudent;
        }
        else
        {
            throw new StudentNotFoundException($"The student with id: {id} was not found");
        }
    }


    public async Task<StudentHomework?> GetByStudentAndHomework(int studentId, int homeworkId)
    {
        var student = await _appDbContext.Students
    .Include(s => s.StudentHomeworks)
        .ThenInclude(sh => sh.Homework)
    .FirstOrDefaultAsync(s => s.ID == studentId);

        var studentHomework = student?.StudentHomeworks
            .FirstOrDefault(sh => sh.HomeworkId == homeworkId);


        return studentHomework;
    }

    public void GradeHomework(StudentHomework studentHomework, int grade)
    {
        if (studentHomework == null)
            throw new ArgumentNullException(nameof(studentHomework), "StudentHomework cannot be null.");

        if (grade < 0 || grade > 100)
            throw new ArgumentOutOfRangeException(nameof(grade), "Grade must be between 0 and 100.");

        studentHomework.Grade = grade;
        //studentHomework.SubmissionDate ??= DateTime.UtcNow;
        //studentHomework.IsCompleted = true;

        _appDbContext.SaveChangesAsync();
    }


    public void SubmitHomework(StudentHomework studentHomework)
    {
        if (studentHomework == null)
            throw new ArgumentNullException(nameof(studentHomework), "StudentHomework cannot be null.");

        //if (studentHomework.IsCompleted)
        //    throw new InvalidOperationException("Homework has already been submitted.");
        var student = studentHomework.Student;
        var courseId = studentHomework.Homework.CourseId;
        var studentCourse = student.StudentCoruses.FirstOrDefault(sc => sc.CourseId == courseId);

        studentHomework.IsCompleted = true;
        studentHomework.SubmissionDate = DateTime.UtcNow;
        studentCourse.ParticipationPoints += 1;

    }

    public void AddGrade(int grade, Student student, Course course)
    {

        if (student != null)
        {
            var studentGrade = student.Grades.FirstOrDefault(g => g.Course.ID== course.ID);

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
        course.Teacher.TaughtCourse.StudentCourses.Add(studentCourse);
        
        _appDbContext.StudentCourses.Add(studentCourse);
        _appDbContext.StudentGPAs.Add(studentGpa);
        _appDbContext.StudentGrades.Add(studentGrade);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(EnrollIntoCourse), true);
    }

    public void AddAbsence(Student student, Absence absence)
    {
        // Ensure absence and course are not null
        if (absence == null)    
        {
            throw new InvalidAbsenceException("Absence cannot be null.");
        }

        if (absence.Course == null)
        {
            throw new InvalidAbsenceException("The course in the absence cannot be null.");
        }

        // Ensure StudentCourses collection is not null
        if (absence.Course.StudentCourses == null)
        {
            throw new InvalidAbsenceException($"The student courses for the course \"{absence.Course.Name}\" cannot be null.");
        }

        // Check if any student in the course matches the given student ID
        if (!absence.Course.StudentCourses.Any(sc => sc.Student != null && sc.Student.ID == student.ID))
        {
            AbsenceException.LogError();
            Logger.LogMethodCall(nameof(AddAbsence), false);
            throw new InvalidAbsenceException($"Cannot mark student {student.Name} as absent in \"{absence.Course.Name}\" because the student is not enrolled in it.");
        }

        // Ensure the student's absences collection is not null
        if (student.Absences == null)
        {
            throw new InvalidAbsenceException("The student's absences list cannot be null.");
        }

        // Check for duplicate absence
        if (student.Absences.Any(d => d.Date == absence.Date && d.Course != null && d.Course.Subject == absence.Course.Subject))
        {
            Logger.LogMethodCall(nameof(AddAbsence), false);
            throw new DuplicateAbsenceException($"Cannot mark student {student.Name} as absent twice on the same day ({absence.Date.ToString("dd/MM/yyyy")}) for the same course ({absence.Course.Name}).");
        }

        // Log the method call and add the absence
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

    public async Task<Student?> GetByName(string name)
    {
        Logger.LogMethodCall(nameof(GetByName), true);
        return await _appDbContext.Students.FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<List<Student>> GetAll()
    {
        return await _appDbContext.Students
             .Include(s => s.Absences)
             .Include(s => s.Grades)
                 .ThenInclude(sg => sg.Course)
             .Include(s => s.GPAs)
             .ToListAsync();
    }


    public async Task<List<Student>> GetByNames(string name)
    {
        return await _appDbContext.Students
        .Where(s => s.Name.ToLower().Contains(name.ToLower()))
        .Include(s => s.Absences)
        .Include(s => s.Grades)
            .ThenInclude(sg => sg.Course)
        .Include(s => s.GPAs)
        .ToListAsync();
    }

    public async Task<List<Student>>GetWithQuery(string query, int page, int pageSize)
    {
        IQueryable<Student> studentsQuery = _appDbContext.Students
            .Include(s => s.Absences)
            .Include(s => s.Grades)
                .ThenInclude(g => g.Course)
            .Include(s => s.GPAs); ;

        if (!string.IsNullOrEmpty(query))
        {
            studentsQuery = studentsQuery.Where(s => s.Name.ToLower().Contains(query.ToLower()));
        }

        return await studentsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCount(string query)
    {
        IQueryable<Student> studentsQuery = _appDbContext.Students;

        if (!string.IsNullOrEmpty(query))
        {
            studentsQuery = studentsQuery.Where(s => s.Name.ToLower().Contains(query.ToLower()));
        }

        return await studentsQuery.CountAsync();
    }


    public async Task<Student?> GetByEmail(string email)
    {
        return await _appDbContext.Students.FirstOrDefaultAsync(s => s.ParentEmail == email);
    }

    public async Task<List<StudentHomework>> GetSubmissionsByHomeworkId(int homeworkId)
    {
        return await _appDbContext.StudentHomework
            .Include(sh => sh.Student)
            .Where(sh => sh.HomeworkId == homeworkId && sh.FileUrl != null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetAllWithGradesAsync()
    {
        return await _appDbContext.Students
            .Include(s => s.Grades)
            .ToListAsync();
    }


    public async Task<IEnumerable<ClassStudentLeaderboardDto>> GetLeaderboardData()
    {
        return await _appDbContext.Students
            .Include(s => s.Grades)
            .Include(s => s.Classroom)
            .Include(s => s.StudentCoruses)
            .Select(s => new StudentLeaderboardEntryDto
            {
                StudentId = s.ID,
                StudentName = s.Name,
                AverageGrade = s.Grades
                    .Where(g => g.GradeValues != null && g.GradeValues.Count > 0)
                    .SelectMany(g => g.GradeValues)
                    .DefaultIfEmpty()
                    .Average(x => (float?)x) ?? 0,
                ParticipationPoints = s.StudentCoruses.Sum(sc=>sc.ParticipationPoints),
                ClassName = s.Classroom.Name
            })
            .GroupBy(s => s.ClassName)
            .Select(g => new ClassStudentLeaderboardDto
            {
                ClassName = g.Key,
                Students = g
                    .OrderByDescending(s => s.AverageGrade + s.ParticipationPoints)
                    .ToList()
            })
            .ToListAsync();
    }
}
