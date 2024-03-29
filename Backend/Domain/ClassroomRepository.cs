using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;

namespace Backend.Infrastructure;
public class ClassroomRepository : IClassroomRepository
{

    private List<Classroom> Classrooms { get; set; }

    public ClassroomRepository()
    {
        Classrooms = new List<Classroom>();
    }

    //Db mock
    public void AddClassroom(Classroom classroom)
    {
        Classrooms.Add(classroom);
    }

    public void DeleteClassroomById(int classroomId)
    {
        Classrooms.Remove(Classrooms.First(classroom => classroom.ID == classroomId));
    }

    public List<Classroom> GetAllClassroom()
    {
        return Classrooms;
    }

    public Classroom GetClassroomById(int classroomId)
    {
        if (!Classrooms.Any(classroom => classroom.ID == classroomId))
        {
            throw new StudentNotFoundException($"Student with ID: {classroomId} not found!");
        }
        return Classrooms.First(classroom => classroom.ID == classroomId);
    }

    public void UpdateClassroom(int classroomId)
    {
        throw new NotImplementedException();
    }
}