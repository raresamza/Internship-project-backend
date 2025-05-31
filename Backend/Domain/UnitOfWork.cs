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
    public UnitOfWork(AppDbContext appDbContext,ISchedulePdfBuilder pdfBuilder ,IScheduleRepository scheduleRepository ,
        ICatalogueRepository catalogueRepository, IAbsenceRepository absenceRepository, IClassroomRepository classroomRepository,
        ICourseRepository courseRepository, ISchoolRepository schoolRepository, IStudentRepository studentRepository, ITeacherRepository teacherRepository,
        IHomeworkRepository homeworkRepository, INotificationRepository notificationRepository,IMailService mailService)
    {
        _appDbContext = appDbContext;
        PdfBuilder = pdfBuilder;    
        CatalogueRepository = catalogueRepository;
        AbsenceRepository = absenceRepository;
        ScheduleRepository = scheduleRepository;
        ClassroomRepository = classroomRepository;
        CourseRepository = courseRepository;
        SchoolRepository = schoolRepository;
        StudentRepository = studentRepository;
        TeacherRepository = teacherRepository;
        HomeworkRepository= homeworkRepository;
        NotificationRepository = notificationRepository;
        MailService = mailService;
    }

    public IMailService MailService { get; private set; }
    public INotificationRepository NotificationRepository { get; private set; }
    public IHomeworkRepository HomeworkRepository { get; private set; }

    public ICatalogueRepository CatalogueRepository { get; private set; }

    public IScheduleRepository ScheduleRepository { get; private set; }
    public IAbsenceRepository AbsenceRepository { get; private set; }
    public IClassroomRepository ClassroomRepository { get; private set; }
    public ICourseRepository CourseRepository { get; private set; }

    public ISchedulePdfBuilder PdfBuilder { get; private set; }
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
