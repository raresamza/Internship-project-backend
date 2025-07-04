using System;
using Backend.Application.Abstractions;
using Backend.Domain.Models;
using Backend.Infrastructure;
using Backend.Infrastructure.Contexts;
using Backend.Infrastructure.SeedData;
using Backend.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity;
using WebApi.Config;
using WebApi.Extensions;
using WebApi.Middleware;
using WebApi.Options;
using WebApi.Services;

using QuestPDF.Infrastructure;
using Backend.Infrastructure.Services;
using static WebApi.Controllers.AccountController;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.
builder.RegisterAuthentication();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IFileStorageRepository, AzureBlobStorageRepository>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddRepositories();
builder.Services.AddMediatR();
builder.Services.AddDbContext();
builder.Services.AddHostedService<UpcomingDeadlineNotificationService>();
builder.Services.AddHostedService<WeeklyReportService>();
builder.Services.AddScoped<ISchedulePdfBuilder, SchedulePdfBuilder>();
builder.Services.AddScoped<IStudentReportService, StudentReportService>();
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IHomeworkRepository, HomeworkRepository>();
//builder.Services.AddAutoMapper(typeof(GradePredictionMappingProfile));
builder.Services.AddScoped<GradePredictionService>();



//builder.Services.AddJsonOptions();
builder.Services.AddSingleton<IdentityService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Allow the frontend's origin
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTiming();

app.UseHttpsRedirection();

// Enable CORS before authentication and authorization
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<AppDbContext>();

//    DbSeeder.SeedDummyData(context); 
//}

app.Run();


static async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var role in Enum.GetNames(typeof(Role)))
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}