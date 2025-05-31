using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public interface IUnitOfWork
{
    public IAbsenceRepository AbsenceRepository { get; }
    public IClassroomRepository ClassroomRepository { get; }
    public ICourseRepository CourseRepository { get; }
    public ISchoolRepository SchoolRepository { get; }

    public IMailService MailService { get; }
    public INotificationRepository NotificationRepository { get; }
    public IHomeworkRepository HomeworkRepository { get; }
    public ISchedulePdfBuilder PdfBuilder { get; }  
    public IScheduleRepository ScheduleRepository { get; }
    public IStudentRepository StudentRepository { get; }

    public ITeacherRepository TeacherRepository { get; }
    public ICatalogueRepository CatalogueRepository { get; }
    Task SaveAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
