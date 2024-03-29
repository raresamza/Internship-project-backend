using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    public interface ITeacherService
    {
        public void AddToClassroom(int classroomID, int teacherID);
        public void AssignToCourse(int courseID, int teacherID);

    }
}
