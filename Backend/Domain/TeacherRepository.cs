using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using MediatR;
using Backend.Exceptions.TeacherException;
using Backend.Application.Courses.Actions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.AbsenceException;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Teachers.Responses;

namespace Backend.Infrastructure;
public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _appDbContext;

    public TeacherRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<Teacher> AssignToCourse(Course course, Teacher teacher)
    {
        if (teacher.Subject == course.Subject)
        {
            if (!_appDbContext.Courses.Local.Any(c => c.ID == course.ID))
            {
                _appDbContext.Courses.Attach(course);
            }

            if (!_appDbContext.Teachers.Local.Any(t => t.ID == teacher.ID))
            {
                _appDbContext.Teachers.Attach(teacher);
            }

            course.Teacher = teacher;
            course.TeacherId = teacher.ID;  // Ensure TeacherId is set

            teacher.TaughtCourse = course;
            teacher.TaughtCourseId = course.ID;  // Ensure TaughtCourseId is set

            Console.WriteLine("Before SaveChangesAsync");
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine("After SaveChangesAsync");

            return teacher;
        }
        else
        {
            TeacherException.LogError();
            throw new TeacherSubjectMismatchException($"The subject that the teacher specializes in: {teacher.Subject} does not match with the course subject: {course.Subject}");
        }
    }


    //Db mock
    public async  Task<Teacher> Create(Teacher teacher)
    {
        _appDbContext.Teachers.Add(teacher);
        await _appDbContext.SaveChangesAsync();
        Logger.LogMethodCall(nameof(Create), true);
        return teacher;
        
    }

    public async Task Delete(Teacher teacher)
    {
        _appDbContext.Teachers.Remove(teacher);
        await _appDbContext.SaveChangesAsync();
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public async Task<List<Teacher>> GetAll()
    {
        return await _appDbContext.Teachers
            .Include(t => t.TaughtCourse)
            .ToListAsync();
    }

    public async Task<Teacher?> GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return await _appDbContext.Teachers
            .Include(t =>t.TaughtCourse)
            .ThenInclude(c => c.StudentCourses)
            .ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(t => t.ID == id);
    }

    //public int GetLastId()
    //{
    //    if (_teachers.Count == 0) return 1;
    //    var lastId = _teachers.Max(a => a.ID);
    //    return lastId + 1;
    //}

    public async Task<Teacher> UpdateTeacher(TeacherUpdateDto teacher, int id)
    {
        var oldTeacher =await  _appDbContext.Teachers.FirstOrDefaultAsync(s => s.ID == id);
        if (oldTeacher != null)
        {
            oldTeacher.Name=teacher.Name;
            oldTeacher.Address=teacher.Address;
            oldTeacher.Age=teacher.Age;
            oldTeacher.Subject=teacher.Subject;
            //oldTeacher = teacher;
            await _appDbContext.SaveChangesAsync();
            return oldTeacher;
        }
        else
        {
            throw new TeacherNotFoundException($"The teacher with id: {id} was not found");
        }
    }
}

