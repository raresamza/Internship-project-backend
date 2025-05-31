using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using Backend.Domain.Exceptions.HomeworkException;
using Backend.Domain.Models;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class HomeworkRepository : IHomeworkRepository
{

    private readonly AppDbContext _appDbContext;


    public HomeworkRepository(AppDbContext app)
    {
        _appDbContext = app;
    }

    public async Task<List<Homework>> GetAllHomeworks()
    {
        var homeworks = await _appDbContext.Homework
            .ToListAsync();

        if (homeworks == null)
        {
            throw new HomeworkNotFoundException("No homeworks found");
        }

        return homeworks;
    }

    public async Task<Homework?> GetById(int id)
    {
        var homework = await _appDbContext.Homework.FirstOrDefaultAsync(x => x.ID == id);

        if (homework == null)
        {
            throw new HomeworkNotFoundException($"Homework with id ${id} not found");
        }

        return homework;
    }


    public void AssignHomeworkToCourse(Course course, string title, string description, DateTime deadline)
    {
        var homework = new Homework
        {
            Title = title,
            Description = description,
            Deadline = deadline,
            Course = course,
            CourseId = course.ID

        };

        course.Homeworks.Add(homework);

        foreach (var studentCourse in course.StudentCourses)
        {
            var studentHomework = new StudentHomework
            {
                StudentId = studentCourse.StudentId,
                Homework = homework,
                IsCompleted = false
            };

            _appDbContext.StudentHomework.Add(studentHomework);
        }
    }

    public async Task<List<Homework>> GetHomeworksDueOnDateAsync(DateTime date)
    {
        return await _appDbContext.Homework
            .Include(h => h.StudentHomeworks)
                .ThenInclude(sh => sh.Student)
            .Where(h => h.Deadline.Date == date.Date)
            .ToListAsync();
    }

    //public async Task<List<StudentHomework>> GetSubmissionsByHomeworkId(int homeworkId)
    //{
    //    return await _appDbContext.StudentHomework
    //        .Include(sh => sh.Student)
    //        .Where(sh => sh.HomeworkId == homeworkId && sh.FileUrl != null)
    //        .ToListAsync();
    //}
}
