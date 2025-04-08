using AutoMapper;
using Backend.Application.Homeworks.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.StudentHomeworks;

public class StudentHomeworkProfile : Profile
{
    public StudentHomeworkProfile() 
    {
        CreateMap<StudentHomework, HomeworkSubmissionDto>()
    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name));

    }
}
