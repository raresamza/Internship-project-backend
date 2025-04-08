using AutoMapper;
using Backend.Application.Homeworks.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.HomeworkProfiles;

public class HomeworkProfile : Profile
{
    public HomeworkProfile()
    {
        CreateMap<Homework, HomeworkDto>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.ID))
            .ForMember(dest => dest.Title, src => src.MapFrom(x => x.Title))
            .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
            .ForMember(dest => dest.Deadline, src => src.MapFrom(x => x.Deadline))
            .ForMember(dest => dest.Grade, src => src.MapFrom(x => x.Grade))
            .ForMember(dest => dest.StudentHomeworks, opt => opt.MapFrom(src => src.StudentHomeworks))
;

    }
}