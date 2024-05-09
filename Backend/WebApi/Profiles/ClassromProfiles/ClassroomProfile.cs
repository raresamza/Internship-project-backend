using AutoMapper;
using Backend.Application.Catalogues.Response;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.ClassromProfiles;

public class ClassroomProfile : Profile
{
    public ClassroomProfile()
    {
        CreateMap<Classroom, ClassroomDto>()
          .ForMember(dest => dest.ClassroomCourses, src => src.MapFrom(x => x.ClassroomCourses))
          .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
          .ForMember(dest => dest.Students, src => src.MapFrom(x => x.Students))
          .ForMember(dest => dest.Teachers, src => src.MapFrom(x => x.Teachers))
          .ForMember(dest => dest.ID, src => src.MapFrom(x => x.ID));
    }
}
