using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.TeacherException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.ClassroomException;

namespace Backend.Infrastructure;
public class ClassroomRepository : IClassroomRepository
{
    private readonly List<Classroom> _classes=new();

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
            classroom.Students.Remove(student);
        }
    }

    public void RemoveTeacher(Teacher teacher, Classroom classroom)
    {
        if (!classroom.Teachers.Contains(teacher))
        {
            Logger.LogMethodCall(nameof(RemoveTeacher), false);
            throw new TeacherNotFoundException($"Cannot remove teacher from this classroom(He is not teaching to this classroom).\nTeacher:{teacher.ToString()}");
        }
        else
        {
            classroom.Teachers.Remove(teacher);
            Logger.LogMethodCall(nameof(RemoveTeacher), true);

        }
    }

    public void AssignCourse(Course course, Classroom classroom)
    {

        if (course == null)
        {
            Logger.LogMethodCall(nameof(AssignCourse), false);
            throw new NullCourseException("This course is not valid");
        }
        else if (classroom.Courses.Contains(course))
        {
            Logger.LogMethodCall(nameof(AssignCourse), false);
            throw new NullCourseException($"The course {course.Name} is already assigned to the classroom {classroom.Name}");
        }
        Logger.LogMethodCall(nameof(RemoveStudent), true);
        classroom.Courses.Add(course);
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
            Logger.LogMethodCall(nameof(AddStudent), true);

        }
        else
        {
            Logger.LogMethodCall(nameof(AddStudent), false);

            throw new StudentAlreadyEnrolledException($"Student {student.Name} already is part of a classroom");
        }
    }

    public void AddTeacher(Teacher teacher, Classroom classroom)
    {

        if (classroom.Teachers.Contains(teacher))
        {
            TeacherException.LogError();
            Logger.LogMethodCall(nameof(AddTeacher), false);
            throw new TeacherAlreadyAssignedException($"Cannot add duplicate teacher to this classroom.\nTeacher:{teacher.ToString()}");
        }
        foreach (Teacher teacher1 in classroom.Teachers)
        {
            if (teacher1.Subject == teacher.Subject)
            {
                TeacherException.LogError();
                Logger.LogMethodCall(nameof(AddTeacher), false);
                throw new TeacherAlreadyAssignedException($"Someone is already teaching: {teacher.Subject} for class {teacher}");
            }
        }
        Logger.LogMethodCall(nameof(AddTeacher), true);
        classroom.Teachers.Add(teacher);
    }

    public Classroom Create(Classroom classroom)
    {
        _classes.Add(classroom);
        return classroom;
    }

    public void Delete(Classroom classroom)
    {
        _classes.Remove(classroom);
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public Classroom? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _classes.FirstOrDefault(c => c.ID == id);
    }

    public int GetLastId()
    {
        if (_classes.Count == 0) return 1;
        var lastId = _classes.Max(a => a.ID);
        return lastId + 1;
    }

    public Classroom UpdateClassroom(Classroom classroom, int id)
    {
        var oldClassroom = _classes.FirstOrDefault(s => s.ID == id);
        if (oldClassroom != null)
        {
            oldClassroom = classroom;

            return oldClassroom;
        }
        else
        {
            throw new NullClassroomException($"The classroom with id: {id} was not found");
        }
    }
}