using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;
using MediatR;

namespace Backend.Application.Notifications.Queries;

public record GetUnreadNotificationsCount() : IRequest<int>;

public class GetUnreadNotificationsCountHandler : IRequestHandler<GetUnreadNotificationsCount, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUnreadNotificationsCountHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(GetUnreadNotificationsCount request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.NotificationRepository.GetUnreadCountAsync();
    }
}