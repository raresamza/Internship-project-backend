using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Infrastructure.Contexts;

namespace Backend.Infrastructure.SeedData;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated(); // Optional: ensures DB exists

        if (context.Students.Any())
            return; // Already seeded

        // Call the method here
        SeedDummyData(context);
    }

    public static void SeedDummyData(AppDbContext context)
    {

        Console.WriteLine("Seeding started...");

        //if (context.Students.Any()) return; // Avoid reseeding


        //Schools
        var school = new School
        {
            Name = "Central High School"
        };
        context.Schools.Add(school);
        context.SaveChanges();

        // Classrooms
        var classroomA = new Classroom { Name = "12A", School = school };
        var classroomB = new Classroom { Name = "12B", School = school};
        context.Classrooms.AddRange(classroomA, classroomB);

        // Subjects (Assume enum or model exists)
        var math = Subject.MATH;
        var english = Subject.ENGLISH;
        var physics = Subject.PHYSICS;
        var chemistry = Subject.CHEMISTRY;
        var history = Subject.HISTORY;

        // Teachers
        var teachers = new[]
        {
        new Teacher { Name = "Mr. Newton", Age = 45, PhoneNumber = 111111111, Address = "Physics Wing", Subject = physics },
        new Teacher { Name = "Mrs. Curie", Age = 40, PhoneNumber = 222222222, Address = "Chemistry Lab", Subject = chemistry },
        new Teacher { Name = "Mr. Einstein", Age = 50, PhoneNumber = 333333333, Address = "Math Dept", Subject = math },
        new Teacher { Name = "Ms. Shakespeare", Age = 38, PhoneNumber = 444444444, Address = "English Dept", Subject = english },
        new Teacher { Name = "Ms. Heritage", Age = 36, PhoneNumber = 555555555, Address = "History Dept", Subject = history }
    };
        context.Teachers.AddRange(teachers);

        // Courses
        var courses = new[]
        {
        new Course() { Teacher = teachers[2],Name="Mathematics",Subject=Subject.MATH },
        new Course() {Teacher = teachers[3], Name = "English Literature", Subject=Subject.ENGLISH},
        new Course() {Teacher = teachers[0], Name = "Physics", Subject=Subject.PHYSICS },
        new Course() {Teacher = teachers[1], Name = "Chemistry", Subject=Subject.CHEMISTRY},
        new Course() {Teacher = teachers[4], Name = "World History", Subject=Subject.HISTORY}
    };
        context.Courses.AddRange(courses);

        // ClassroomCourses: link every course to both classrooms
        var classroomCourses = new List<ClassroomCourse>();
        foreach (var course in courses)
        {
            classroomCourses.Add(new ClassroomCourse { Course = course, Classroom = classroomA });
            classroomCourses.Add(new ClassroomCourse { Course = course, Classroom = classroomB });
        }
        context.ClassroomCourses.AddRange(classroomCourses);

        // Students
        var students = new[]
        {
        new Student
        {
            Name = "Alice Johnson",
            Age = 17,
            PhoneNumber = 999999001,
            Address = "123 Main St",
            ParentName = "Maria Johnson",
            ParentEmail = "maria.johnson@example.com",
            Assigned = true,
            Classroom = classroomA
        },
        new Student
        {
            Name = "Bob Smith",
            Age = 18,
            PhoneNumber = 999999002,
            Address = "456 Maple Ave",
            ParentName = "Tom Smith",
            ParentEmail = "tom.smith@example.com",
            Assigned = true,
            Classroom = classroomA
        },
        new Student
        {
            Name = "Cathy Lee",
            Age = 17,
            PhoneNumber = 999999003,
            Address = "789 Oak Dr",
            ParentName = "Sarah Lee",
            ParentEmail = "sarah.lee@example.com",
            Assigned = true,
            Classroom = classroomB
        }
    };
        context.Students.AddRange(students);

        // StudentCourses: Enroll every student in all courses
        var studentCourses = new List<StudentCourse>();
        foreach (var student in students)
        {
            foreach (var course in courses)
            {
                studentCourses.Add(new StudentCourse
                {
                    Student = student,
                    Course = course
                });
            }
        }
        context.StudentCourses.AddRange(studentCourses);

        context.SaveChanges();
        Console.WriteLine("Seeding completed.");

    }


}
