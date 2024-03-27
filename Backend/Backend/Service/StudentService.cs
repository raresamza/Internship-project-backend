using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.Placeholders;
using Backend.Models;
using Backend.Repository;
using Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backend.Service
{
    public class StudentService : ISchoolService
    {

        private readonly StudentRepository _studentRepository;

        public StudentService(StudentRepository repository)
        {
            _studentRepository = repository;
        }

        public void AddGrade(int grade, Course course,int studentID,List<Student> students)
        {

            Student dbStudent = _studentRepository.GetStudentById(studentID,students);

            bool checkIfPresent = dbStudent.Grades.TryGetValue(course, out var list);
            if (checkIfPresent)
            {
                Message.GradeMessage(grade, dbStudent, course.Name);
                Logger.LogMethodCall(nameof(AddGrade), true);
                list.Add(grade);
                //mock db update
            }
            else
            {
                StudentException.LogError();
                Logger.LogMethodCall(nameof(AddGrade), false);
                throw new StudentException($"Student {dbStudent.Name} is not enrolled into the course: {course.Name}, therefor he can not be assigned a grade for this course");
            }
        }

        public void EnrollIntoCourse(Course course, int studentID, List<Student> students)
        {

            Student dbStudent = _studentRepository.GetStudentById(studentID, students);

            if (course.Students.Contains(dbStudent))
            {
                StudentException.LogError();
                throw new StudentException($"Student {dbStudent.Name} is already enrolled into this course");
            }
            course.Students.Add(dbStudent);
            List<int> grades = new List<int>();
            dbStudent.GPAs.Add(course, 0);
            dbStudent.Grades.Add(course, grades);
            //mock db update
        }

        public void AddAbsence(Absence absence, int studentID, List<Student> students)
        {
            Student dbStudent = _studentRepository.GetStudentById(studentID, students);


            if (!absence.Course.Students.Contains(dbStudent))
            {
                AbsenceException.LogError();
                Logger.LogMethodCall(nameof(AddAbsence), false);
                throw new InvalidAbsenceException($"Cannot mark student {dbStudent.Name} as absent in \"{absence.Course.Name}\" because student is not enrolled in it");
            }
            else if (dbStudent.Absences.Any(d => d.Date == absence.Date && d.Course.Subject == absence.Course.Subject))
            {
                Logger.LogMethodCall(nameof(AddAbsence), false);
                throw new DuplicateAbsenceException($"Cannot mark student {dbStudent.Name} as absent twice in the same day ({absence.Date.ToString("dd/MM/yyyy")}) for the same course ({absence.Course.Name})");
            }
            Message.AbsenceMessage(dbStudent, absence);
            Logger.LogMethodCall(nameof(AddAbsence), true);
            dbStudent.Absences.Add(absence);
            //mock db update
        }

    }
}
