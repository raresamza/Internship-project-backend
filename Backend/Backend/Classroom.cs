using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Classroom


    {
        public string Name { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
        public Classroom(string name)
        {
            Name = name;
        }

        public void assignCourse(Course course)
        {
            if(course == null)
            {
                throw new CourseException("This course is not valid");
            } else if (Courses.Contains(course))
            {
                throw new CourseException($"The course {course.Name} is already assigned to the classroom {Name}");
            }
            Courses.Add(course);
        }
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();

        public void addTeacher(Teacher teacher)
        {
            if (Teachers.Contains(teacher))
            {
                TeacherException.LogError();
                throw new TeacherException($"Cannot add duplicate teacher to this classroom.\nTeacher:{teacher.ToString()}");
            }
            foreach (Teacher teacher1 in Teachers)
            {
                if (teacher1.Subject==teacher.Subject)
                {
                    TeacherException.LogError();
                    throw new TeacherException($"Someone is already teaching: {teacher.Subject} for class {Name}");
                }
            }
            Teachers.Add(teacher);
        }
        public void addStudent(Student student)
        {
            if (Students.Contains(student))
            {
                StudentException.LogError();
                throw new StudentException($"Cannot add duplicate student to this classroom.\nStudent:{student.ToString()}");
            }
            if (!student.Assigned)
            {
                Students.Add(student);
                student.Assigned = true;
            }
            else
            {
                throw new StudentException($"Student {Name} already is part of a classroom");
            }
        }
        public void removeTeacher(Teacher teacher)
        {
            if (!Teachers.Contains(teacher))
            {
                TeacherException.LogError();
                throw new Exception($"Cannot add duplicate teacher to this classroom(He is not teaching to this classroom).\nTeacher:{teacher.ToString()}");
            }
            else
            {
                Teachers.Remove(teacher);
            }
        }
        public void removeStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                TeacherException.LogError();
                throw new Exception($"Cannot delete student from this classroom(he is not in this classroom).\nStudent:{student.ToString()}");
            }
            else
            {
                Students.Remove(student);
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"This classrooms has the following teachers:\n");
            foreach(Teacher teacher in Teachers)
            {
                stringBuilder.Append($"\t{teacher.ToString()}");
            }
            stringBuilder.Append($"This classrooms has the following students:");
            foreach (Student student in Students)
            {
                stringBuilder.Append($"\t{student.ToString()}");
            }
            return stringBuilder.ToString();
        }
    }
}
