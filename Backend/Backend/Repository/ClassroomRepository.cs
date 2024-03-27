using Backend.Exceptions.StudentException;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public class ClassroomRepository : IClassroomRepository
    {
        //Db mock
        public void AddClassroom(Classroom classroom)
        {
            throw new NotImplementedException();
        }

        public void DeleteClassroomById(int classroomId)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAllClassroom()
        {
            throw new NotImplementedException();
        }

        public Classroom GetClassroomById(int classroomId, List<Classroom> classrooms)
        {
            if (!classrooms.Any(classroom => classroom.ID == classroomId))
            {
                throw new StudentNotFoundException($"Student with ID: {classroomId} not found!");
            }
            return classrooms.First(classroom => classroom.ID == classroomId);
        }

        public void UpdateClassroom(int classroomId)
        {
            throw new NotImplementedException();
        }
    }
}
