using AutoMapper;
using Backend.Application.Catalogues.Response;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.ClassromProfiles;

public class TeacherClassroomProfile : Profile
{
    public TeacherClassroomProfile()
    {
        CreateMap<TeacherClassroom, TeacherClassroomDto>()
          .ForMember(dest => dest.TeacherId, src => src.MapFrom(x => x.TeacherId))
          .ForMember(dest => dest.Teacher, src => src.Ignore())
          .ForMember(dest => dest.TeacherName, src => src.MapFrom(x => x.Teacher.Name))
          .ForMember(dest => dest.ClassroomId, src => src.MapFrom(x => x.ClassroomId));
    }
}
