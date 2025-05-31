using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using MediatR;

namespace Backend.Application.Notifications.Queries;
public record MarkNotificationAsRead(int Id) : IRequest<Unit>;
public class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsRead,Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkNotificationAsReadHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(MarkNotificationAsRead request, CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(request.Id);
        if (notification != null)
        {
            notification.IsRead = true;
            await _unitOfWork.SaveAsync();
        }

        return Unit.Value;
    }
}