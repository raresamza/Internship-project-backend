using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Notifications.Response;
using MediatR;

namespace Backend.Application.Notifications.Queries;

public record GetAllNotifications() : IRequest<List<NotificationDto>>;

public class GetAllNotificationsHandler : IRequestHandler<GetAllNotifications, List<NotificationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllNotificationsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<NotificationDto>> Handle(GetAllNotifications request, CancellationToken cancellationToken)
    {
        var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
        return notifications.Select(_mapper.Map<NotificationDto>).ToList();
    }
}