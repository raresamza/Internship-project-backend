using Backend.Domain.Models;
using Backend.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using WebApi.Config;
using WebApi.Extensions;
using WebApi.Middleware;
using WebApi.Options;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.RegisterAuthentication();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IEmailSenderService, EmailService>();

builder.Services.AddRepositories();
builder.Services.AddMediatR();
builder.Services.AddDbContext();
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

app.Run();
