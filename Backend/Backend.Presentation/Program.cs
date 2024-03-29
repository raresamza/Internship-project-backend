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
using Backend.Application.Courses.Create;
using Backend.Application.Teachers.Actions;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Actions;

var diContainer = new ServiceCollection()
    .AddSingleton<IStudentRepository, StudentRepository>()
    .AddSingleton<ITeacherRepository, TeacherRepository>()
    .AddSingleton<ICourseRepository, CourseRepository>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ICourseRepository).Assembly))
    .BuildServiceProvider();

var mediator = diContainer.GetRequiredService<IMediator>();

var student = await mediator.Send(new CreateStudent("mail@mail.com", "Adi", 11, 1111111, "Rares", "deva"));
var student2 = await mediator.Send(new CreateStudent("email@email.com", "Ady", 22, 2222222, "Rares Amza", "geva"));
var course = await mediator.Send(new CreateCourse("Physics I", Subject.PHYSICS));
var teacher = await mediator.Send(new CreateTeacher(50, 41235235, "Bebu", "deva", Subject.PHYSICS));
await mediator.Send(new AddStudent(student.ID, course.ID));
await mediator.Send(new AddStudent(student2.ID, course.ID));
await mediator.Send(new AssignTeacherToCourse(course.ID, teacher.ID));

Console.WriteLine(await mediator.Send(new GetCourseById(course.ID)));
Console.WriteLine(await mediator.Send(new GetStudentById(student.ID)));
Console.WriteLine(await mediator.Send(new GetStudentById(student2.ID)));
//var updatedTeacher=
//Console.WriteLine(await mediator.Send(new GetTeacherById(teacher.ID)));


