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
        public void AddGrade(int grade, Course course,int  studentID);

        public void EnrollIntoCourse(Course course);

        public void AddAbsence(Absence absence);
    }
}
