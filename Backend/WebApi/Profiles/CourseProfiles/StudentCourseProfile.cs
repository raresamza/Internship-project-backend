using AutoMapper;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.CourseProfiles;

public class StudentCourseProfile : Profile
{
    public StudentCourseProfile()
    {
        CreateMap<StudentCourse, StudentCourseDto>()
            .ForMember(dest => dest.StudentId, src => src.MapFrom(x => x.StudentId))
            .ForMember(dest => dest.Student, src => src.Ignore())
            .ForMember(dest => dest.StudentName, src => src.MapFrom(x => x.Student.Name));
    }
}
