using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.CourseProfiles;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Subject, src => src.MapFrom(x => x.Subject))
            .ForMember(dest => dest.ID, src => src.MapFrom(x => x.ID))
            .ForMember(dest => dest.StudentCourses, src => src.MapFrom(x => x.StudentCourses))
            .ForMember(dest => dest.TeacherId, src => src.MapFrom(x => x.TeacherId))
            .ForMember(dest => dest.TeacherName, src => src.MapFrom(x => x != null ? x.Teacher.Name : "Unknown"));
    }
}
