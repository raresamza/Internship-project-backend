using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.CourseException;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;


namespace Backend.Infrastructure;
public class CourseRepository : ICourseRepository
{

    private readonly AppDbContext _appDbContext;


    public CourseRepository(AppDbContext app)
    {
        _appDbContext = app;
    }

    public async Task<Course> Create(Course course)
    {
        _appDbContext.Courses.Add(course);
        await _appDbContext.SaveChangesAsync();
        return course;
    }

    public async Task<Course> GetCourseById(int courseId, List<Course> courses)
    {
        var course = await _appDbContext.Courses.FirstOrDefaultAsync(c => c.ID == courseId);

        if (course == null)
        {
            throw new StudentNotFoundException($"Course with ID: {courseId} not found!");
        }

        return course;
    }

    

    public async Task<Course?> GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return await _appDbContext.Courses
                       .Include(c => c.Teacher) 
                       .Include(c =>c.StudentCourses)
                            .ThenInclude(sc => sc.Student)
                       .FirstOrDefaultAsync(c => c.ID == id);
    }

    public async Task<Course> UpdateCourse(Course course, int id)
    {
        var courseToUpdate = await _appDbContext.Courses.FirstOrDefaultAsync(c => c.ID == course.ID);

        if (courseToUpdate == null)
        {
            throw new NullCourseException($"Course with ID: {course.ID} not found");
        }

        courseToUpdate.Name = course.Name;
        courseToUpdate.Subject = course.Subject;
        courseToUpdate.Teacher = course.Teacher;
        courseToUpdate.TeacherId= course.TeacherId;

        await _appDbContext.SaveChangesAsync();

        return courseToUpdate;
    }

    public async Task DeleteCourse(Course course)
    {
        var absencesToRemove = _appDbContext.Absences
        .Where(a => a.CourseId == course.ID)
        .ToList();

        _appDbContext.Absences.RemoveRange(absencesToRemove);
        _appDbContext.Courses.Remove(course);


        //_appDbContext.ClassroomCourses.Remove(_appDbContext.ClassroomCourses.Find(course.ID));
        //_appDbContext.StudentCourses.Remove(_appDbContext.StudentCourses.Find(course.ID));
        //_appDbContext.StudentGrades.Remove(_appDbContext.StudentGrades.Find(course.ID));
        //_appDbContext.Absences.Remove(course);
        //_appDbContext.StudentGPAs.Remove
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(DeleteCourse), true);
    }

    public async void Add(Student student, int s)
    {
        Course? course = await GetById(s);
        var studentCourse = new StudentCourse
        {
            Course = course,
            CourseId = course.ID,
            StudentId = student.ID,
            Student = student,
        };
        course.StudentCourses.Add(studentCourse);
        _appDbContext.StudentCourses.Add(studentCourse);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(Add), true);
    }

    public async Task<List<Course>> GetAll()
    {
        return await _appDbContext.Courses
                       .Include(c => c.Teacher)
                       .Include(c => c.StudentCourses)
                            .ThenInclude(sc => sc.Student)
                       .ToListAsync();
    }
}
