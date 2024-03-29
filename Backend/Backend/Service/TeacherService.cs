//using Backend.Exceptions.ClassroomException;
//using Backend.Exceptions.Placeholders;
//using Backend.Exceptions.TeacherException;
//using Backend.Domain.Models;
//using Backend.;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace Backend.Service;

//public class TeacherService : ITeacherService
//{

//    private readonly TeacherRepository _teacherRepository;
//    private readonly ClassroomRepository _classroomRepository;
//    private readonly CourseRepository _courseRepository;

//    public TeacherService(TeacherRepository teacherRepository, ClassroomRepository classroomRepository, CourseRepository courseRepository)
//    {
//        _teacherRepository = teacherRepository;
//        _classroomRepository = classroomRepository;
//        _courseRepository = courseRepository;
//    }

//    public void AddToClassroom(int classroomId, int teacherID)
//    {

//        Teacher dbTeacher = _teacherRepository.GetTeacherById(teacherID);
//        Classroom dbClassroom = _classroomRepository.GetClassroomById(classroomId);

//        if (dbClassroom == null)
//        {
//            throw new NullClassroomException("This classroom is invalid");
//        }
//        if (dbClassroom.Teachers.Contains(dbTeacher))
//        {
//            throw new TeacherAlreadyAssignedException($"Cannot assign {dbTeacher.Name} teacher twice to the same class");
//        }
//        if (dbClassroom.Teachers.Any(t => t.Subject == dbTeacher.Subject))
//        {
//            throw new TeacherSubjectMismatchException($"A teacher teaching {dbTeacher.Subject} is already assigned");
//        }
//        dbClassroom.Teachers.Add(dbTeacher);
//    }

//    public void AssignToCourse(int courseID, int teacherID, List<Teacher> teachers, List<Course> courses)
//    {
//        Teacher dbTeacher = _teacherRepository.GetTeacherById(teacherID);
//        Course dbCourse = _courseRepository.GetCourseById(courseID);

//        if (dbTeacher.Subject == dbCourse.Subject)
//        {
//            dbCourse.Teacher = dbTeacher;
//            dbTeacher.TaughtCourse = dbCourse;
//        }
//        else
//        {
//            TeacherException.LogError();
//            throw new TeacherSubjectMismatchException($"The subject that the teacher spelcializes in: {dbTeacher.Subject} does not match with the course subject: {dbCourse.Subject}");
//        }

//    }
//}
