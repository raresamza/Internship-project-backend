using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public interface IClassroomRepository
    {
        public Classroom GetClassroomById(int classroomId, List<Classroom> classrooms);
        public void DeleteClassroomById(int classroomId);
        public List<Student> GetAllClassroom();
        public void UpdateClassroom(int classroomId);
        public void AddClassroom(Classroom classroom);
    }
}
