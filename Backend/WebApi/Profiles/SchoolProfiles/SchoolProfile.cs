using AutoMapper;
using Backend.Application.Schools.Response;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;

namespace WebApi.Profiles.SchoolProfiles;

public class SchoolProfile : Profile
{
    public SchoolProfile()
    {
        CreateMap<UpdateSchoolDto, SchoolDto>()
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Classrooms, src => src.MapFrom(x => x.Classrooms))
            .ForMember(dest => dest.ID, src => src.MapFrom(x => x.ID));
    }
}
