using AutoMapper;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;


public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.ParentName, src => src.MapFrom(x => x.ParentName))
            .ForMember(dest => dest.ParentEmail, src => src.MapFrom(x => x.ParentEmail))
            .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address))
            .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
            .ForMember(dest => dest.Age, src => src.MapFrom(x => x.Age))
            .ForMember(dest => dest.ID, src => src.MapFrom(x => x.ID))
            .ForMember(dest => dest.Absences, src => src.MapFrom(x => x.Absences))
            .ForMember(dest => dest.GPAs, src => src.MapFrom(x => x.GPAs))
            //.ForMember(dest => dest.ParticipationPoints, src => src.MapFrom(x => x.ParticipationPoints))
            .ForMember(dest => dest.Grades, src => src.MapFrom(x => x.Grades));
    }
}
