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
using Backend.Application.Schools.Delete;
using Backend.Application.Schools.Queries;
using Backend.Application.Classrooms.Queries;
using Backend.Application.Absences.Queries;
using Backend.Application.Courses.Delete;
using Backend.Application.Students.Update;

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
//var teacher = await mediator.Send(new CreateTeacher(50, 41235235, "Bebu12", "deva21", Subject.MATH));
//var absence = await mediator.Send(new CreateAbsence(2));
//var absence1 = await mediator.Send(new CreateAbsence(course1.ID));
//var classroom = await mediator.Send(new CreateClassroom("12A",1));
//var catalogue= await mediator.Send(new CreateCatalogue(2));
//var school = await mediator.Send(new CreateSchool("Colegiul National Decebal Deva"));

//await mediator.Send(new EnrollIntoCourse(1,2));
//await mediator.Send(new EnrollIntoCourse(student2.ID,course.ID));
//await mediator.Send(new EnrollIntoCourse(student.ID,course1.ID));
//await mediator.Send(new UpdateStudent(1, new Student { Address = "Deva City", Age=1, Name = "Rarez", ParentEmail= "parent email", ParentName="Parent Nmae", PhoneNumber=1 }));
//await mediator.Send(new AssignTeacherToCourse(2,1));

//await mediator.Send(new AddTeacherToClassroom(1,1));
//await mediator.Send(new AddStudentToClassroom(1, 1));
//await mediator.Send(new AddStudentToClassroom(student2.ID, classroom.ID));

//await mediator.Send(new AddGrade(1,2,10));
//await mediator.Send(new AddGrade(1,2,7));
//await mediator.Send(new AddGrade(student.ID,course.ID,3));
//await mediator.Send(new RemoveGrade(1,1,10));
//await mediator.Send(new AddGrade(student.ID,course.ID,8));
//await mediator.Send(new AddAbsence(1,2));
//await mediator.Send(new AddAbsence(student.ID, absence1.Id));
//await mediator.Send(new MotivateAbsence(1,1,1));
//await mediator.Send(new AddGpa(1, 1));
//await mediator.Send(new AddClassroom(1,1));
//await mediator.Send(new RemoveClassroom(1,2));
//await mediator.Send(new AddAbsence(student.ID, absence.Id));

//Console.WriteLine(await mediator.Send(new GetCourseById(1)));
//Console.WriteLine(await mediator.Send(new GetTeacherById(1)));
Console.WriteLine(await mediator.Send(new DeleteCourse(4)));
//Console.WriteLine(await mediator.Send(new GetStudentById(1)));
//Console.WriteLine(await mediator.Send(new DeleteSchool(1)));
//Console.WriteLine(await mediator.Send(new GetStudentById(student2.ID)));
//Console.WriteLine(await mediator.Send(new GetAbsenceById(absence.Id)));
//Console.WriteLine(await mediator.Send(new GetClassroomById(3)));
//Console.WriteLine(await mediator.Send(new GetCatalogueById(catalogue.ID)));
//Console.WriteLine(await mediator.Send(new GetSchoolById(1)));
//Console.WriteLine(await mediator.Send(new GetAbsenceByDateAndCourse(DateTime.Today,1,1)));