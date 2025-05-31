using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class NotificationRepository : INotificationRepository
{

    private readonly AppDbContext _appDbContext;

    public NotificationRepository(AppDbContext app)
    {
        _appDbContext = app;
    }

    public async Task AddNotificationAsync(Notification notification)
    {
        _appDbContext.Notifications.Add(notification);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Notification> Create(Notification notification)
    {
        _appDbContext.Notifications.Add(notification);
        await _appDbContext.SaveChangesAsync();
        return notification;
    }

    public async Task<List<Notification>> GetAllAsync()
    {
        return await _appDbContext.Notifications.ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _appDbContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<int> GetUnreadCountAsync()
    {
        return await _appDbContext.Notifications
            .Where(n => !n.IsRead)
            .CountAsync();
    }
}
