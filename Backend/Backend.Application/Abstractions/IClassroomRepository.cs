using Backend.Domain.Models;

namespace Backend.Application.Abstractions;
public interface IClassroomRepository
{
    public Classroom GetClassroomById(int classroomId);
    public void DeleteClassroomById(int classroomId);
    public List<Classroom> GetAllClassroom();
    public void UpdateClassroom(int classroomId);
    public void AddClassroom(Classroom classroom);
}

