using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Backend.Models;
using Backend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backend.Service
{
    public class ClassroomService : IClassroomService
    {
        private readonly ClassroomRepository _classroomRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        public ClassroomService(ClassroomRepository classroomRepository, TeacherRepository teacherRepository, StudentRepository studentRepository, CourseRepository courseRepository)
        {
            _classroomRepository = classroomRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }
        public void AddStudent(int studentID, List<Student> students, int classroomID, List<Classroom> classrooms)
        {

            Student dbStudent = _studentRepository.GetStudentById(studentID, students);

            Classroom dbClassroom = _classroomRepository.GetClassroomById(classroomID, classrooms);

            if (dbClassroom.Students.Contains(dbStudent))
            {
                StudentException.LogError();
                throw new StudentAlreadyEnrolledException($"Cannot add duplicate student to this classroom.\nStudent:{student.ToString()}");
            }
            if (!dbStudent.Assigned)
            {
                dbClassroom.Students.Add(dbStudent);
                dbStudent.Assigned = true;
            }
            else
            {
                throw new StudentAlreadyEnrolledException($"Student {dbStudent.Name} already is part of a classroom");
            }
        }

        public void AddTeacher(int teacherID, List<Teacher> teachers, int classroomID, List<Classroom> classrooms)
        {
            Teacher dbTeacher = _teacherRepository.GetTeacherById(teacherID, teachers);

            Classroom dbClassroom = _classroomRepository.GetClassroomById(classroomID, classrooms);

            if (dbClassroom.Teachers.Contains(dbTeacher))
            {
                TeacherException.LogError();
                throw new TeacherAlreadyAssignedException($"Cannot add duplicate teacher to this classroom.\nTeacher:{dbTeacher.ToString()}");
            }
            foreach (Teacher teacher1 in dbClassroom.Teachers)
            {
                if (teacher1.Subject == dbTeacher.Subject)
                {
                    TeacherException.LogError();
                    throw new TeacherAlreadyAssignedException($"Someone is already teaching: {dbTeacher.Subject} for class {dbTeacher}");
                }
            }
            dbClassroom.Teachers.Add(dbTeacher);
        }

        public void AssignCourse(int courseID, int classroomID, List<Classroom> classrooms, List<Course> courses)
        {
            Classroom dbClassroom = _classroomRepository.GetClassroomById(classroomID, classrooms);

            Course dbCourse = _courseRepository.GetCourseById(courseID, courses);

            if (dbCourse == null)
            {
                throw new NullCourseException("This course is not valid");
            }
            else if (dbClassroom.Courses.Contains(dbCourse))
            {
                throw new NullCourseException($"The course {dbCourse.Name} is already assigned to the classroom {dbClassroom.Name}");
            }
            dbClassroom.Courses.Add(dbCourse);
        }

        public void RemoveStudent(int classroomID, int studentID, List<Student> students, List<Classroom> classrooms)
        {
            Student dbStudent = _studentRepository.GetStudentById(studentID, students);

            Classroom dbClassroom = _classroomRepository.GetClassroomById(classroomID, classrooms);


            if (!dbClassroom.Students.Contains(dbStudent))
            {
                TeacherException.LogError();
                throw new StudentNotFoundException($"Cannot delete student from this classroom(he is not in this classroom).\nStudent:{dbStudent.ToString()}");
            }
            else
            {
                dbClassroom.Students.Remove(dbStudent);
            }
        }

        public void RemoveTeacher(int classrommID, int teacherID, List<Classroom> classrooms, List<Teacher> teachers)
        {

            Classroom dbClassroom = _classroomRepository.GetClassroomById(classrommID, classrooms);
            Teacher dbTeacher = _teacherRepository.GetTeacherById(teacherID, teachers);


            if (!dbClassroom.Teachers.Contains(dbTeacher))
            {
                TeacherException.LogError();
                throw new TeacherNotFoundException($"Cannot remove teacher from this classroom(He is not teaching to this classroom).\nTeacher:{dbTeacher.ToString()}");
            }
            else
            {
                dbClassroom.Teachers.Remove(dbTeacher);
            }
        }
    }
}
