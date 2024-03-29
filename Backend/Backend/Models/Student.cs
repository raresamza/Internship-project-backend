using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;

namespace Backend.Domain.Models;
public class Student : User

//add school id to all classes and add to all methods check to see if from same school.
{
    //add dictionary of gpa's for final grade

    //public int classroomID { get; set; } = -1;

    //public Student(string parentEmail, string parentName, int age, int phoneNumber, string name, string address) : base(age, phoneNumber, name, address)
    //{
    //    ID = ++_lastAssignedId;
    //    ParentName = parentName;
    //    ParentEmail = parentEmail;
    //}

    public Student() : base()
    {
        ID = ++_lastAssignedId;
        Absences = _absences;
        Grades = _grades;
        GPAs = _gpas;
    }

    private static int _lastAssignedId = 0;

    public int ID { get; }

    public bool Assigned { get; set; } = false;

    private readonly List<Absence> _absences = new();
    public ICollection<Absence> Absences { get; set; }

    public required string ParentEmail { get; set; }

    public required string ParentName { get; set; }

    private readonly Dictionary<Course, List<int>> _grades = new();
    private readonly Dictionary<Course, decimal> _gpas = new();


    public Dictionary<Course, List<int>> Grades { get; set; }
    public Dictionary<Course, decimal> GPAs { get; set; }

    //public void MotivateAbsence(DateTime date, Course course)
    //{
    //    if (course == null)
    //    {
    //        CourseException.LogError();
    //        throw new NullCourseException("This course is not valid");
    //    }
    //    else if (!course.Students.Contains(this))
    //    {
    //        StudentException.LogError();
    //        throw new StudentNotEnrolledException($"Cannot motivate absence for {Name} because he is not enrolled into {course.Name}");
    //    }
    //    foreach (Absence absence in Absences)
    //    {
    //        if (absence.Date == date.Date && absence.Course.Name.Equals(course.Name))
    //        {
    //            Absences.Remove(absence);
    //        }
    //    }

    //    //Absences.Remove(getAbsenceByDate(date));
    //}

    //public void AddAbsence(Absence absence)
    //{
    //    //foreach (Student s in absence.Course.Students)
    //    //{
    //    //    Console.WriteLine("\n\n\n\n\t\t\t"+s.ToString());
    //    //}
    //    //Console.WriteLine($"print pentru a vedea daca student e in array: {absence.Course.Students.Contains(this)}");
    //    if (!absence.Course.Students.Contains(this))
    //    {
    //        AbsenceException.LogError();
    //        Logger.LogMethodCall(nameof(AddAbsence), false);
    //        throw new InvalidAbsenceException($"Cannot mark student {Name} as absent in \"{absence.Course.Name}\" because student is not enrolled in it");
    //    }
    //    else if (Absences.Any(d => d.Date == absence.Date && d.Course.Subject == absence.Course.Subject))
    //    {
    //        Logger.LogMethodCall(nameof(AddAbsence), false);
    //        throw new DuplicateAbsenceException($"Cannot mark student {Name} as absent twice in the same day ({absence.Date.ToString("dd/MM/yyyy")}) for the same course ({absence.Course.Name})");
    //    }
    //    Message.AbsenceMessage(this, absence);
    //    Logger.LogMethodCall(nameof(AddAbsence), true);
    //    Absences.Add(absence);
    //}

    //public void addToClassroom(Classroom classroom) {
    //    if(!(classroomID==-1))
    //    {
    //        classroom.addStudent(this);
    //    } else
    //    {
    //        ClassroomException.LogError();
    //        throw new ClassroomException($"Student {Name} is already part of class {classroom.Name}");
    //    } 
    //}


    //public void removeFromClassroom(Classroom classroom)
    //{
    //    if (classroomID == -1 || !classroom.Students.Contains(this))
    //    {
    //        ClassroomException.LogError();
    //        throw new ClassroomException($"Cannot remove {Name} from class because he isn't part of it");
    //    }
    //    classroom.Students.Remove(this);
    //}

    //public void enrollStudnet(Course course) {
    //    List<int> grades = new List<int>();
    //    Grades.Add(course, grades);
    //}
    //public void AddGrade(int grade, Course course)
    //{

    //    bool checkIfPresent = Grades.TryGetValue(course, out var list);
    //    if (checkIfPresent)
    //    {
    //        Message.GradeMessage(grade, this, course.Name);
    //        Logger.LogMethodCall(nameof(AddGrade), true);
    //        list.Add(grade);
    //    }
    //    else
    //    {
    //        StudentException.LogError();
    //        Logger.LogMethodCall(nameof(AddGrade), false);
    //        throw new StudentException($"Student {Name} is not enrolled into the course: {course.Name}, therefor he can not be assigned a grade for this course");
    //    }
    //}

    //public void AddGpa(decimal grade, Course course)
    //{ 
    //    bool checkIfPresent = GPAs.TryGetValue(course, out var GPA);
    //    if (checkIfPresent)
    //    {
    //        GPA = grade;
    //        GPAs[course] = GPA;
    //    }
    //    else
    //    {
    //        StudentException.LogError();
    //        throw new StudentException($"Student {Name} is not enrolled into the course: {course.Name}, therefor the GPA cannot be computed");
    //    }

    //}

    //public void RemoveGrade(int grade, Course course)
    //{
    //    bool checkIfPresent = Grades.TryGetValue(course, out var list);
    //    if (checkIfPresent)
    //    {
    //        list.Remove(grade);
    //    }
    //    else
    //    {
    //        StudentException.LogError();
    //        throw new StudentException($"Student is not enrolled into the course {course.Name}");
    //    }
    //}
    //public void EnrollIntoCourse(Course course)
    //{
    //    if (course.Students.Contains(this))
    //    {
    //        StudentException.LogError();
    //        throw new StudentException($"Student {Name} is already enrolled into this course");
    //    }
    //    course.Students.Add(this);
    //    List<int> grades = new List<int>();
    //    GPAs.Add(course, 0);
    //    Grades.Add(course, grades);

    //    //Console.WriteLine(course.Students);
    //}
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"\nStudent(ID: {ID}) details:\n\tStundent Name: {Name}\n\tStudent Age: {Age}\n\tStudent Phone Number: {PhoneNumber}\n\tStudent's Parent Name: {ParentName}\n\tStudent's Parent Email Addrees: {ParentEmail}\n\tStudent Address: {Address}\n\tStudent Grades:\n");
        foreach (var entry in Grades)
        {
            Course course = entry.Key;
            List<int> grades = entry.Value;
            stringBuilder.Append($"\t\tCourse: {course.Name}\n");

            stringBuilder.Append("\t\t\tGrades: ");
            foreach (var grade in grades)
            {
                stringBuilder.Append($"{grade} ");
            }
            stringBuilder.Append("\n\t\t\tGPA: ");
            stringBuilder.Append($"{(GPAs.TryGetValue(course, out decimal res) == true ? res : "N/A")} ");
            stringBuilder.Append('\n');

        }
        stringBuilder.Append($"\t\tAbsences:\n");

        foreach (Absence absence in Absences)
        {
            stringBuilder.Append($"\t\t\t{absence.Date.ToString("dd/MM/yyyy")}, {absence.Course.Name}\n");
        }

        return stringBuilder.ToString();
    }

    //maybe add list of courses

}