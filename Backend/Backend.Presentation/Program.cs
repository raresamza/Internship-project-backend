using Backend.Application.Abstractions;
using Backend.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Backend.Application.Students.Create;
using Backend.Application.Students.Queries;
using Backend.Application.Teachers.Create;
using Backend.Domain.Models;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Queries;
using Backend.Application.Teachers.Actions;
using Backend.Application.Courses.Actions;
using Backend.Application.Absences.Create;
using Backend.Application.Students.Actions;
using Backend.Application.Classrooms.Create;
using Backend.Application.Catalogues.Create;
using Backend.Application.Classrooms.Actions;
using Backend.Application.Catalogues.Actions;
using Backend.Application.Schools.Create;
using Backend.Application.Schools.Actions;
using Backend.Infrastructure.Contexts;
using Backend.Application.Teachers.Queries;

var diContainer = new ServiceCollection()
    .AddSingleton<IStudentRepository, StudentRepository>()
    .AddSingleton<ITeacherRepository, TeacherRepository>()
    .AddSingleton<ICourseRepository, CourseRepository>()
    .AddSingleton<IAbsenceRepository, AbsenceRepository>()
    .AddSingleton<IClassroomRepository, ClassroomRepository>()
    .AddSingleton<ICatalogueRepository, CatalogueRepository>()
    .AddDbContext<AppDbContext>()
    .AddSingleton<ISchoolRepository, SchoolRepository>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ICourseRepository).Assembly))
    .BuildServiceProvider();

var mediator = diContainer.GetRequiredService<IMediator>();

//var student = await mediator.Send(new CreateStudent("mail@mail.com", "Adi", 11, 1111111, "Rares", "deva"));
//var student2 = await mediator.Send(new CreateStudent("email@email.com", "Ady", 22, 2222222, "Rares Amza", "geva"));
//var course = await mediator.Send(new CreateCourse("Physics I", Subject.PHYSICS));
//var course1 = await mediator.Send(new CreateCourse("Math I", Subject.MATH));
//var teacher = await mediator.Send(new CreateTeacher(50, 41235235, "Bebu", "deva", Subject.PHYSICS));
//var absence = await mediator.Send(new CreateAbsence(1));
//var absence1 = await mediator.Send(new CreateAbsence(course1.ID));
//var classroom = await mediator.Send(new CreateClassroom("12A",1));
//var catalogue= await mediator.Send(new CreateCatalogue(2));
//var school = await mediator.Send(new CreateSchool("Colegiul National Decebal Deva"));

//await mediator.Send(new EnrollIntoCourse(1, 1));
//await mediator.Send(new EnrollIntoCourse(student2.ID,course.ID));
//await mediator.Send(new EnrollIntoCourse(student.ID,course1.ID));

//await mediator.Send(new AssignTeacherToCourse(1, 1));

//await mediator.Send(new AddTeacherToClassroom(1,2));
//await mediator.Send(new AddStudentToClassroom(2,2));
//await mediator.Send(new AddStudentToClassroom(student2.ID, classroom.ID));

//await mediator.Send(new AddGrade(1,1,7));
//await mediator.Send(new AddGrade(student.ID,course.ID,3));
//await mediator.Send(new RemoveGrade(1,1,10));
//await mediator.Send(new AddGrade(student.ID,course.ID,8));
//await mediator.Send(new AddAbsence(1,1));
//await mediator.Send(new AddAbsence(student.ID, absence1.Id));
//await mediator.Send(new MotivateAbsence(student.ID, absence.Id,absence.Course.ID));
//await mediator.Send(new AddGpa(1, 1));
//await mediator.Send(new AddClassroom(school.ID,classroom.ID));
//await mediator.Send(new RemoveClassroom(school.ID, classroom.ID));
//await mediator.Send(new AddAbsence(student.ID, absence.Id));

//Console.WriteLine(await mediator.Send(new GetCourseById(1)));
//Console.WriteLine(await mediator.Send(new GetTeacherById(1)));
Console.WriteLine(await mediator.Send(new GetStudentById(1)));
//Console.WriteLine(await mediator.Send(new GetStudentById(student2.ID)));
//Console.WriteLine(await mediator.Send(new GetAbsenceById(absence.Id)));
//Console.WriteLine(await mediator.Send(new GetClassroomById(classroom.ID)));
//Console.WriteLine(await mediator.Send(new GetCatalogueById(catalogue.ID)));
//Console.WriteLine(await mediator.Send(new GetSchoolById(school.ID)));
//Console.WriteLine(await mediator.Send(new GetAbsenceByDateAndCourse(DateTime.Today,course.ID,student.ID)));