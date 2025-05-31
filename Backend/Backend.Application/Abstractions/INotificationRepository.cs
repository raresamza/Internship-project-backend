using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;

namespace Backend.Application.Abstractions;

public interface INotificationRepository
{
    Task<Notification> Create(Notification notification);
    Task AddNotificationAsync(Notification notification);
    Task<List<Notification>> GetAllAsync();
    Task<Notification?> GetByIdAsync(int id);
    Task<int> GetUnreadCountAsync();
}
