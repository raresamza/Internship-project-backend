using AutoMapper;
using Backend.Application.Courses.Response;
using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;

namespace WebApi.Profiles.TeacherProfiles;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>()
           .ForMember(dest => dest.TaughtCourseId, src => src.MapFrom(x => x.TaughtCourseId))
           .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address))
           .ForMember(dest => dest.Age, src => src.MapFrom(x => x.Age))
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
           .ForMember(dest => dest.Subject, src => src.MapFrom(x => x.Subject))
           .ForMember(dest => dest.ID, src => src.MapFrom(x => x.ID))
           .ForMember(dest => dest.StudentCourses, src => src.MapFrom(x => x.TaughtCourse.StudentCourses))
           .ForMember(dest => dest.TaughtCourse, src => src.MapFrom(x => x.TaughtCourse))
           .ForMember(dest => dest.CourseName, src => src.MapFrom(x => x.TaughtCourse.Name));
    }
}
