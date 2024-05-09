using AutoMapper;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;

namespace WebApi.Profiles.Student;

public class GradesProfile : Profile
{
    public GradesProfile()
    {
        CreateMap<StudentGrade, StudentGradeDto>()
            .ForMember(dest => dest.GradeValues, src => src.MapFrom(x => x.GradeValues))
            .ForMember(dest => dest.Student, src => src.Ignore())
            .ForMember(dest => dest.CourseId, src => src.MapFrom(x => x.CourseId))
            .ForMember(dest => dest.CourseName, src => src.MapFrom(x => x.Course.Name))
            .ForMember(dest => dest.Course, src => src.Ignore());

        CreateMap<List<StudentGrade>, ICollection<StudentGradeDto>>()
                    .ConvertUsing((src, dest, context) => context.Mapper.Map<List<StudentGrade>, List<StudentGradeDto>>(src));
    }
}
