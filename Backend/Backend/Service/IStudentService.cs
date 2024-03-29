using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    public interface IStudentService
    {
        public void AddGrade(int grade, Course course, int studentID);

        public void EnrollIntoCourse(Course course, int studentID);

        public void AddAbsence(Absence absence, int studentID);
        public void AddGpa(decimal grade, Course course, int studentId);

        public void RemoveGrade(int grade, Course course, int studentId);
        public void MotivateAbsence(DateTime date, Course course, int studentId);
    }
}
