using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Application.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure;

public class ScheduleRepository : IScheduleRepository
{

    private readonly AppDbContext _appDbContext;

    private readonly ILogger<ScheduleRepository> _logger;
    public ScheduleRepository(AppDbContext app, ILogger<ScheduleRepository> logger)
    {

        _appDbContext = app;
        _logger = logger;
    }

    public List<TimeSlot> GenerateTimeSlots(TimeSpan start, TimeSpan end, TimeSpan classLength, TimeSpan breakLength)
    {
        var slots = new List<TimeSlot>();

        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            if (day is DayOfWeek.Saturday or DayOfWeek.Sunday)
                continue;

            var current = start;
            while (current + classLength <= end)
            {
                slots.Add(new TimeSlot
                {
                    Day = day,
                    StartTime = current
                });
                current += classLength + breakLength;
            }
        }

        return slots;
    }

    public List<ScheduleEntry> GenerateSchedule()
    {
        var schedule = new List<ScheduleEntry>();
        var timeSlots = GenerateTimeSlots(
            TimeSpan.FromHours(8),
            TimeSpan.FromHours(15),
            TimeSpan.FromMinutes(50),
            TimeSpan.FromMinutes(10)
        );

        // Load all course-classroom pairs
        var courseClassroomPairs = _appDbContext.ClassroomCourses
            .Include(cc => cc.Course)
                .ThenInclude(c => c.Teacher)
            .Include(cc => cc.Classroom)
            .ToList();

        // Load all student-course relations
        var studentCourses = _appDbContext.StudentCourses.ToList();

        _logger.LogInformation($"Found {courseClassroomPairs.Count} course/classroom pairs to schedule.");
        _logger.LogInformation($"Generated {timeSlots.Count} timeslots across weekdays.");

        foreach (var pair in courseClassroomPairs)
        {
            var course = pair.Course;
            var classroom = pair.Classroom;

            // Get students enrolled in this course
            var enrolledStudentIds = studentCourses
                .Where(sc => sc.CourseId == course.ID)
                .Select(sc => sc.StudentId)
                .ToList();

            _logger.LogInformation($"Trying to schedule Course '{course.Name}' (ID: {course.ID}) in Classroom '{classroom.Name}' (ID: {classroom.ID})");

            foreach (var slot in timeSlots)
            {
                // Check for teacher, classroom, and student conflicts
                bool conflict = schedule.Any(s =>
                    s.TimeSlot.Day == slot.Day &&
                    s.TimeSlot.StartTime == slot.StartTime &&
                    (
                        s.Course.TeacherId == course.TeacherId ||
                        s.Classroom.ID == classroom.ID ||
                        studentCourses.Any(sc =>
                            enrolledStudentIds.Contains(sc.StudentId) &&
                            sc.CourseId == s.Course.ID)
                    )
                );

                if (!conflict)
                {
                    _logger.LogInformation($"  → Scheduled at {slot.Day} {slot.StartTime:hh\\:mm}");

                    schedule.Add(new ScheduleEntry
                    {
                        TimeSlot = slot,
                        Course = course,
                        Classroom = classroom
                    });

                    break;
                }
                else
                {
                    _logger.LogInformation($"  ✖ Conflict at {slot.Day} {slot.StartTime:hh\\:mm}");
                }
            }
        }

        _logger.LogInformation($"✅ Final schedule contains {schedule.Count} entries.");
        return schedule;
    }

}
