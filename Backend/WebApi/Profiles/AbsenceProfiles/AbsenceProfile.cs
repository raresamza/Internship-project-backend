using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;

namespace WebApi.Profiles.AbsenceProfiles;

public class AbsenceProfile : Profile
{
    public AbsenceProfile()
    {
        CreateMap<Absence, AbsenceDto>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.Date, src => src.MapFrom(x => x.Date))
            .ForMember(dest => dest.Course, src => src.Ignore())
            .ForMember(dest => dest.CourseName, src => src.MapFrom(x => x.Course.Name));
    }
}
