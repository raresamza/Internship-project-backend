using Backend.Application.Abstractions;
using Backend.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    public UnitOfWork(AppDbContext appDbContext,ICatalogueRepository catalogueRepository, IAbsenceRepository absenceRepository, IClassroomRepository classroomRepository, ICourseRepository courseRepository, ISchoolRepository schoolRepository, IStudentRepository studentRepository, ITeacherRepository teacherRepository)
    {
        _appDbContext = appDbContext;
        CatalogueRepository = catalogueRepository;
        AbsenceRepository = absenceRepository;
        ClassroomRepository = classroomRepository;
        CourseRepository = courseRepository;
        SchoolRepository = schoolRepository;
        StudentRepository = studentRepository;
        TeacherRepository = teacherRepository;
    }

    public ICatalogueRepository CatalogueRepository { get; private set; }
    public IAbsenceRepository AbsenceRepository { get; private set; }
    public IClassroomRepository ClassroomRepository { get; private set; }
    public ICourseRepository CourseRepository { get; private set; }
    public ISchoolRepository SchoolRepository { get; private set; }
    public IStudentRepository StudentRepository { get; private set; }
    public ITeacherRepository TeacherRepository { get; private set; }

    public async Task BeginTransactionAsync()
    {
        await _appDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _appDbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _appDbContext.Database.RollbackTransactionAsync();
    }

    public async Task SaveAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }
}
