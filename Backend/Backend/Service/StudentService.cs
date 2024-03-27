using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
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
    public class StudentService : IStudentService
    {

        private readonly StudentRepository _studentRepository;

        public StudentService(StudentRepository repository)
        {
            _studentRepository = repository;
        }

        public void AddGrade(int grade, Course course, int studentID, List<Student> students)
        {

            Student dbStudent = _studentRepository.GetStudentById(studentID, students);

            bool checkIfPresent = dbStudent.Grades.TryGetValue(course, out var list);
            if (checkIfPresent)
            {
                Message.GradeMessage(grade, dbStudent, course.Name);
                Logger.LogMethodCall(nameof(AddGrade), true);
                list.Add(grade);
                //Mock db update
                //Update with grade
                //_studentRepository.UpdateStudent()
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
            //Mock db update
            //Update with grade
            //_studentRepository.UpdateStudent()
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
            //Update with grade
            //_studentRepository.UpdateStudent()
            //Mock db update
        }

        public void AddGpa(decimal grade, Course course, int studentId, List<Student> students)
        {

            Student dbStudent = _studentRepository.GetStudentById(studentId, students);


            //bool checkIfPresent = dbStudent.GPAs.TryGetValue(course, out var GPA);
            if (dbStudent.GPAs.TryGetValue(course, out var GPA))
            {
                GPA = grade;
                dbStudent.GPAs[course] = GPA;
                //Update with grade
                //_studentRepository.UpdateStudent()
            }
            else
            {
                StudentException.LogError();
                throw new StudentException($"Student {dbStudent.Name} is not enrolled into the course: {course.Name}, therefor the GPA cannot be computed");
            }
        }

        public void RemoveGrade(int grade, Course course, int studentId, List<Student> students)
        {

            Student dbStudent = _studentRepository.GetStudentById(studentId, students);

            bool checkIfPresent = dbStudent.Grades.TryGetValue(course, out var list);
            if (checkIfPresent)
            {
                list.Remove(grade);
                //Update with grade
                //_studentRepository.UpdateStudent()
            }
            else
            {
                StudentException.LogError();
                throw new StudentException($"Student is not enrolled into the course {course.Name}");
            }
        }

        public void MotivateAbsence(DateTime date, Course course, int studentId, List<Student> students)
        {

            //need to find course by name;

            Student dbStudent = _studentRepository.GetStudentById(studentId, students);


            if (course == null)
            {
                CourseException.LogError();
                throw new NullCourseException("This course is not valid");
            }
            else if (!course.Students.Contains(dbStudent))
            {
                StudentException.LogError();
                throw new StudentNotEnrolledException($"Cannot motivate absence for {dbStudent.Name} because he is not enrolled into {course.Name}");
            }
            foreach (Absence absence in dbStudent.Absences)
            {
                if (absence.Date == date.Date && absence.Course.Name.Equals(course.Name))
                {
                    dbStudent.Absences.Remove(absence);
                }
            }
        }
    }
}
