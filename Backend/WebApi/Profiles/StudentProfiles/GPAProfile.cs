using AutoMapper;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;

namespace WebApi.Profiles.Student;

public class GPAProfile : Profile
{
    public GPAProfile()
    {
        CreateMap<StudentGPA,StudentGPADto>()
            .ForMember(dest => dest.GPAValue, src => src.MapFrom(x => x.GPAValue))
            .ForMember(dest => dest.Course, src => src.Ignore())
            .ForMember(dest => dest.CourseId, src => src.MapFrom(x => x.CourseId))
            .ForMember(dest => dest.CourseName, src => src.MapFrom(x => x.Course.Name));
    }
}
