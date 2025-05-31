using AutoMapper;
using Backend.Application.Notifications.Response;
using Backend.Application.Schools.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.NotificationProfiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.Message, src => src.MapFrom(x => x.Message))
            .ForMember(dest => dest.IsRead, src => src.MapFrom(x => x.IsRead))
            .ForMember(dest => dest.Type, src => src.MapFrom(x => x.Type))
            .ForMember(dest => dest.CreatedAt, src => src.MapFrom(x => x.CreatedAt));
    }
}
