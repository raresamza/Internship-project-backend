using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Backend.Application.Abstractions;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.Placeholders;
using Backend.Infrastructure.Utils;
using Backend.Exceptions.TeacherException;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;
public class SchoolRepository : ISchoolRepository
{

    private readonly AppDbContext _appDbContext;

    public SchoolRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    //Db mock
    public void AddClassroom(Classroom classroom, School school)
    {
        if (classroom == null)
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(AddClassroom), false);
            throw new NullClassroomException($"Classroom is not valid");
        }
        else if (school.Classrooms.Contains(classroom))
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(AddClassroom), false);
            throw new ClassroomAlreadyRegisteredException($"Classroom {classroom.Name} is already registered");
        }
        Logger.LogMethodCall(nameof(AddClassroom), true);
        school.Classrooms.Add(classroom);

        _appDbContext.SaveChanges();
    }


    public void RemoveClassroom(Classroom classroom, School school)
    {

        Console.WriteLine(classroom.ToString());
        if (classroom == null)
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(RemoveClassroom), false);

            throw new NullClassroomException("Classroom is not valid");
        }
        else if (!school.Classrooms.Contains(classroom))
        {
            ClassroomException.LogError();
            Logger.LogMethodCall(nameof(RemoveClassroom), false);
            throw new ClassroomNotRegisteredException($"Classroom {classroom.Name} cannot be deleted because it is not registered");
        }
        Logger.LogMethodCall(nameof(RemoveClassroom), true);
        var classroomToRemove = school.Classrooms.FirstOrDefault(c => c.ID == classroom.ID);

        foreach (var student in classroomToRemove.Students)
        {
            student.ClassroomId = null;
        }

        _appDbContext.SaveChanges();
        school.Classrooms.Remove(classroomToRemove);
        _appDbContext.SaveChanges();
    }



    public School? GetById(int id)
    {
        Logger.LogMethodCall(nameof(GetById), true);
        return _appDbContext.Schools
            .Include(s => s.Classrooms)
                .ThenInclude(c => c.Students)
                    .ThenInclude(s =>s.Grades)
                        .ThenInclude(g => g.Course)
            .Include(s => s.Classrooms)
                .ThenInclude(c => c.Students)
                    .ThenInclude(s => s.GPAs)
            .Include(s =>s.Classrooms)
                .ThenInclude(c => c.Students)
                    .ThenInclude(s => s.Absences)
            .Include(s => s.Classrooms)
                .ThenInclude(c => c.Teachers)
                    .ThenInclude(c => c.Teacher)
            .FirstOrDefault(s => s.ID == id);
    }

    public School Create(School school)
    {
        _appDbContext.Schools.Add(school);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(Create), true);
        return school;
    }

    public void UpdateSchool(School school, int id)
    {
        var oldSchool = GetById(id);
        if (oldSchool == null)
        {
            Logger.LogMethodCall(nameof(UpdateSchool), false);
            throw new TeacherNotFoundException($"School with id {id} not found");
        }
        //implement mapper
        oldSchool.Name = school.Name;
        oldSchool.Classrooms = school.Classrooms;
        Logger.LogMethodCall(nameof(UpdateSchool), true);
    }

    public void Delete(School school)
    {
        _appDbContext.Schools.Remove(school);
        _appDbContext.SaveChanges();
        Logger.LogMethodCall(nameof(Delete), true);
    }

    public School Update(int schoolId, School school)
    {
        var oldSchool = _appDbContext.Schools.FirstOrDefault(s => s.ID == schoolId);
        if (oldSchool != null)
        {
            oldSchool = school;

            return oldSchool;
        }
        else
        {
            throw new StudentNotFoundException($"The student with id: {schoolId} was not found");
        }
    }

    public IEnumerable<School> GetAll()
    {
        return _appDbContext.Schools;
    }
}

