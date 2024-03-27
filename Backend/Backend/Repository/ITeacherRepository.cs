using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public interface ITeacherRepository
    {
        public Teacher GetTeachertById(int teacherId, List<Teacher> teachers);
        public void DeleteTeacherById(int teacherId);
        public List<Teacher> GetAllTeachers();
        public void UpdateTeacher(int teacherID);
        public void AddTeacher(Teacher teacher);
    }
}
