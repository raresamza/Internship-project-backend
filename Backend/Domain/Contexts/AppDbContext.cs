﻿using Backend.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Contexts;
//IdentityDbContext<User, IdentityRole<int>, int>
public class AppDbContext : IdentityDbContext
{

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }


    public DbSet<StudentHomework> StudentHomework { get; set; }

    public DbSet<Homework> Homework { get; set; } = default!;
    public DbSet<Student> Students { get; set; } = default!;
    public DbSet<Teacher> Teachers { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<Classroom> Classrooms { get; set; } = default!;
    public DbSet<School> Schools { get; set; } = default!;
    public DbSet<Catalogue> Catalogues { get; set; } = default!;
    public DbSet<Absence> Absences { get; set; } = default!;
    public DbSet<StudentGPA> StudentGPAs { get; set; } = default!;

    public DbSet<ClassroomCourse> ClassroomCourses { get; set; } = default!;
    public DbSet<StudentCourse> StudentCourses { get; set; } = default!;
    public DbSet<StudentGrade> StudentGrades { get; set; } = default!;
    public DbSet<TeacherClassroom> TeacherClassrooms { get; set; } = default!;



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(@"Server=localhost;Database=InternshipSchoolSystem;Trusted_Connection=True;TrustServerCertificate=True")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClassroomCourse>()
            .HasKey(cc => new { cc.ClassroomId, cc.CourseId });

        modelBuilder.Entity<ClassroomCourse>()
            .HasOne(cc => cc.Classroom)
            .WithMany(c => c.ClassroomCourses)
            .HasForeignKey(cc => cc.ClassroomId);

        modelBuilder.Entity<ClassroomCourse>()
            .HasOne(cc => cc.Course)
            .WithMany(c => c.ClassroomCourses)
            .HasForeignKey(cc => cc.CourseId);

        modelBuilder.Entity<Student>()
            .HasKey(s => s.ID);

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCoruses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        modelBuilder.Entity<StudentGPA>().HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentGPA>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.GPAs)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentGPA>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.GPAs)
            .HasForeignKey(sc => sc.CourseId);

        modelBuilder.Entity<StudentGrade>().HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentGrade>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentGrade>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.Grades)
            .HasForeignKey(sc => sc.CourseId);

        modelBuilder.Entity<TeacherClassroom>().HasKey(tc => new { tc.TeacherId, tc.ClassroomId });

        modelBuilder.Entity<TeacherClassroom>()
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherClassrooms)
            .HasForeignKey(tc => tc.TeacherId);

        modelBuilder.Entity<TeacherClassroom>()
            .HasOne(tc => tc.Classroom)
            .WithMany(c => c.Teachers)
            .HasForeignKey(tc => tc.ClassroomId);

        modelBuilder.Entity<StudentGPA>()
            .Property(s => s.GPAValue)
            .HasColumnType("decimal(18, 2)");


        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.TaughtCourse)
            .WithOne(c => c.Teacher)
            .HasForeignKey<Course>(c => c.TeacherId) // Foreign key property in the Course entity
            .IsRequired(false);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithOne(t => t.TaughtCourse)
            .HasForeignKey<Teacher>(t => t.TaughtCourseId) // Foreign key property in the Teacher entity
            .IsRequired(false);


        modelBuilder.Entity<StudentHomework>()
            .HasOne(sh => sh.Student)
            .WithMany(s => s.StudentHomeworks)
            .HasForeignKey(sh => sh.StudentId);

        modelBuilder.Entity<StudentHomework>()
            .HasOne(sh => sh.Homework)
            .WithMany(h => h.StudentHomeworks)
            .HasForeignKey(sh => sh.HomeworkId);

        modelBuilder.Entity<StudentHomework>()
    .HasKey(sh => sh.ID);

        modelBuilder.Entity<StudentHomework>()
            .HasOne(sh => sh.Student)
            .WithMany(s => s.StudentHomeworks)
            .HasForeignKey(sh => sh.StudentId);

        modelBuilder.Entity<StudentHomework>()
            .HasOne(sh => sh.Homework)
            .WithMany(h => h.StudentHomeworks)
            .HasForeignKey(sh => sh.HomeworkId);


        base.OnModelCreating(modelBuilder);
    }
}
