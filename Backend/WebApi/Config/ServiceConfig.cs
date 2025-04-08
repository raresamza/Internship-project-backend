using System;
using System.Text.Json.Serialization;
using Backend.Application.Abstractions;
using Backend.Infrastructure;
using Backend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Config;

public static class ServiceConfig
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAbsenceRepository, AbsenceRepository>();
        services.AddScoped<ICatalogueRepository, CatalogueRepository>();
        services.AddScoped<IClassroomRepository, ClassroomRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ISchoolRepository, SchoolRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ICourseRepository).Assembly));
    }

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
    }
    //public static void AddJsonOptions(this IServiceCollection services)
    //{
    //    services.AddControllers()
    //    .AddJsonOptions(options =>
    //    {
    //        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //    });
    //}
}
