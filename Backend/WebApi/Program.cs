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

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.RegisterAuthentication();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IFileStorageRepository, AzureBlobStorageRepository>();
builder.Services.AddScoped<IEmailSenderService, EmailService>();

builder.Services.AddRepositories();
builder.Services.AddMediatR();
builder.Services.AddDbContext();
builder.Services.AddScoped<ISchedulePdfBuilder, SchedulePdfBuilder>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IHomeworkRepository, HomeworkRepository>();


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
