using Backend.Application.Abstractions;
using Backend.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Backend.Application.Students.Create;
using Backend.Application.Students.Queries;
using Backend.Exceptions.StudentException;
using Backend.Application.Teachers.Create;
using Backend.Domain.Models;
using Backend.Application.Teachers.Queries;

var diContainer = new ServiceCollection()
    .AddSingleton<IStudentRepository, StudentRepository>()
    .AddSingleton<ITeacherRepository, TeacherRepository>()
    .AddSingleton<ICourseRepository, CourseRepository>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ICourseRepository).Assembly))
    .BuildServiceProvider();

var mediator = diContainer.GetRequiredService<IMediator>();

var student = await mediator.Send(new CreateStudent("mail@mail.com", "Adi", 11, 1111111, "Rares", "deva"));
Console.WriteLine(await mediator.Send(new GetStudentById(student.ID)));


var teacher = await mediator.Send(new CreateTeacher(50, 41235235, "Bebu", "deva",Subject.PHYSICS));
Console.WriteLine(await mediator.Send(new GetTeacherById(teacher.ID)));


