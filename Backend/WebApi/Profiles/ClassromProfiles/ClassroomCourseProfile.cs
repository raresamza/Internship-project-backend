using AutoMapper;
using Backend.Application.Catalogues.Response;
using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.ClassromProfiles;

public class ClassroomCourseProfile : Profile
{
    public ClassroomCourseProfile()
    {
        CreateMap<ClassroomCourse, ClassroomCourseDto>()
          .ForMember(dest => dest.ClassroomId, src => src.MapFrom(x => x.ClassroomId))
          .ForMember(dest => dest.CourseId, src => src.MapFrom(x => x.CourseId))
          .ForMember(dest => dest.Course, src => src.Ignore())
          .ForMember(dest => dest.Classroom, src => src.Ignore());
    }
}
