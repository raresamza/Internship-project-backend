using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.TeacherException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.ClassroomException;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;
public class ClassroomRepository : IClassroomRepository
{
    private readonly AppDbContext _appDbContext;

    public ClassroomRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void RemoveStudent(Student student, Classroom classroom)
    {
        if (!classroom.Students.Contains(student))
        {
            Logger.LogMethodCall(nameof(RemoveStudent), false);
            throw new StudentNotFoundException($"Cannot delete student from this classroom(he is not in this classroom).\nStudent:{student.ToString()}");
        }
        else
        {
            Logger.LogMethodCall(nameof(RemoveStudent), true);
            classroom.Students.FirstOrDefault(s => s.ID == student.ID).ClassroomId = null;
            student.Assigned = false;
            _appDbContext.SaveChanges();
            classroom.Students.Remove(student);
            _appDbContext.SaveChanges();
        }
    }

    public void RemoveTeacher(Teacher teacher, Classroom classroom)
    {
        if (!classroom.Teachers.Any(t => t.Teacher == teacher))
        {
            Logger.LogMethodCall(nameof(RemoveTeacher), false);
            throw new TeacherNotFoundException($"Cannot remove teacher from this classroom(He is not teaching to this classroom).\nTeacher:{teacher.ToString()}");
        }
        else
        {
            var teacherClassroom=classroom.Teachers.FirstOrDefault(tc => tc.TeacherId == teacher.ID);
            if (teacherClassroom != null)
            {
                classroom.Teachers.Remove(teacherClassroom);
            }
            Logger.LogMethodCall(nameof(RemoveTeacher), true);

        }
        _appDbContext.SaveChanges();
    }

    public void AssignCourse(Course course, Classroom classroom)
    {

        if (course == null)
        {
            Logger.LogMethodCall(nameof(AssignCourse), false);
            throw new NullCourseException("This course is not valid");
        }
        else if (classroom.ClassroomCourses.Any(cc => cc.Classroom == classroom))
        {
            Logger.LogMethodCall(nameof(AssignCourse), false);
            throw new NullCourseException($"The course {course.Name} is already assigned to the classroom {classroom.Name}");
        }
        Logger.LogMethodCall(nameof(RemoveStudent), true);
        classroom.ClassroomCourses.Add(
            new ClassroomCourse 
            { 
                Classroom=classroom, 
                Course=course,
                ClassroomId=classroom.ID, 
                CourseId=course.ID 
            });

        _appDbContext.SaveChanges();
    }

    public void AddStudent(Student student, Classroom classroom)
    {

        if (classroom.Students.Contains(student))
        {
            StudentException.LogError();
            Logger.LogMethodCall(nameof(AddStudent),false);
            throw new StudentAlreadyEnrolledException($"Cannot add duplicate student to this classroom.\nStudent:{student.ToString()}");
        }
        if (!student.Assigned)
        {
            classroom.Students.Add(student);
            student.Assigned = true;
            _appDbContext.SaveChanges();
            Logger.LogMethodCall(nameof(AddStudent), true);
        }
        else
        {

            Logger.LogMethodCall(nameof(AddStudent), false);

            throw new StudentAlreadyEnrolledException($"Student {student.Name} already is part of a classroom");
        }

        //_appDbContext.SaveChanges();
    }

    public void AddTeacher(Teacher teacher, Classroom classroom)
    {

        if (classroom.Teachers.Any(cc => cc.TeacherId == teacher.ID))
        {
            TeacherException.LogError();
            Logger.LogMethodCall(nameof(AddTeacher), false);
            throw new TeacherAlreadyAssignedException($"Cannot add duplicate teacher to this classroom.\nTeacher:{teacher.ToString()}");
        }
        foreach (TeacherClassroom teacher1 in classroom.Teachers)
        {
            if (teacher1.Teacher.Subject == teacher.Subject)
            {
                TeacherException.LogError();
                Logger.LogMethodCall(nameof(AddTeacher), false);
                throw new TeacherAlreadyAssignedException($"Someone is already teaching: {teacher.Subject} for class {teacher}");
            }
        }
        Logger.LogMethodCall(nameof(AddTeacher), true);


        classroom.Teachers.Add(
            new TeacherClassroom
            {
                Teacher= teacher,
                Classroom= classroom,
                ClassroomId = classroom.ID,
                TeacherId= teacher.ID,
            });

        _appDbContext.SaveChanges();
    }

    public Classroom Create(Classroom classroom)
    {
        _appDbContext.Classrooms.Add(classroom);
        _appDbContext.SaveChanges();
        return classroom;
    }

    public void Delete(Classroom classroom)
    {
        _appDbContext.Classrooms.Remove(classroom);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Classroom? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _appDbContext.Classrooms
            .Include(c => c.Students)
            .Include(c => c.Teachers)  
                .ThenInclude(tc => tc.Teacher)  
            .FirstOrDefault(c => c.ID == id);
    }

    public Classroom UpdateClassroom(Classroom classroom, int id)
    {
        var oldClassroom = _appDbContext.Classrooms.FirstOrDefault(s => s.ID == id);
        if (oldClassroom != null)
        {

            oldClassroom.School = classroom.School;
            oldClassroom.Teachers = classroom.Teachers;
            oldClassroom.Students = classroom.Students;
            oldClassroom.Catalogue = classroom.Catalogue;
            oldClassroom.ClassroomCourses = classroom.ClassroomCourses;
            oldClassroom.Name = classroom.Name;
            _appDbContext.SaveChanges();
            return oldClassroom;
        }
        else
        {
            throw new NullClassroomException($"The classroom with id: {id} was not found");
        }
    }
}