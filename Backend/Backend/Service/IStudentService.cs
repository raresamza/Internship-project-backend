using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    public interface IStudentService
    {
        public void AddGrade(int grade, Course course, int studentID, List<Student> students);

        public void EnrollIntoCourse(Course course, int studentID, List<Student> students);

        public void AddAbsence(Absence absence, int studentID, List<Student> students);
        public void AddGpa(decimal grade, Course course, int studentId, List<Student> students);

        public void RemoveGrade(int grade, Course course, int studentId, List<Student> students);
        public void MotivateAbsence(DateTime date, Course course, int studentId, List<Student> students);
    }
}
