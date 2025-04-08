using AutoMapper;
using Backend.Application.Homeworks.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.HomeworkProfiles;

public class StudentHomeworkProfile : Profile
{
    public StudentHomeworkProfile()
    {
        CreateMap<StudentHomework, StudentHomeworkDto>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.ID))
            .ForMember(dest => dest.StudentId, src => src.MapFrom(x => x.StudentId))
            .ForMember(dest => dest.HomeworkId, src => src.MapFrom(x => x.HomeworkId))
            .ForMember(dest => dest.IsCompleted, src => src.MapFrom(x => x.IsCompleted))
            .ForMember(dest => dest.StudentName, src => src.MapFrom(x =>x.Student.Name))
            .ForMember(dest => dest.HomeworkTitle, src => src.MapFrom(x => x.Homework.Title))
            .ForMember(dest => dest.SubmissionDate, src => src.MapFrom(x => x.SubmissionDate))
            .ForMember(dest => dest.Grade, src => src.MapFrom(x => x.Grade));
    }
}