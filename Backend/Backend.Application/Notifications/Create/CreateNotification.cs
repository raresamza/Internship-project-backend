using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Classrooms.Response;
using Backend.Application.Notifications.Response;
using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Notifications.Create;

public record CreateNotification(string message, int studentId,string title, NotificationType type) : IRequest<NotificationDto>;

public class CreateNotificationHandler :IRequestHandler<CreateNotification, NotificationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateNotificationHandler> _logger;
    public CreateNotificationHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateNotificationHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<NotificationDto> Handle(CreateNotification request, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _unitOfWork.StudentRepository.GetById(request.studentId);
            if (student == null) 
            {
                throw new StudentNotFoundException($"Student with id {request.studentId} was not found");
            }
            var notification = new Notification() { Message = request.message, StudentId=request.studentId,Student=student,CreatedAt=DateTime.UtcNow,Title=request.title,Type=request.type};
            await _unitOfWork.BeginTransactionAsync();
            var createdNotification = await _unitOfWork.NotificationRepository.Create(notification);
            await _unitOfWork.CommitTransactionAsync();
            _logger.LogInformation($"Action in notification at: {DateTime.Now.TimeOfDay}");
            return _mapper.Map<NotificationDto>(createdNotification);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in notification at: {DateTime.Now.TimeOfDay}");
            Console.WriteLine(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
