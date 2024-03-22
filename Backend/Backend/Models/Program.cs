using Backend.Exceptions.Placeholders;
using Backend.Models;

internal class Program
{
    private static void Main(string[] args)
    {


        Student rares = new Student("mail@mail.com", "Adi", 11, 11111111, "Rares", "deva");
        Student rares1 = new Student("mail1@mail.com", "Adi1", 112, 11111112, "Rares1", "deva1");
        Teacher teacher = new Teacher(Subject.MATH, 11, 11111111, "Monea", "Deva");
        Course math = new Course("Math I", Subject.MATH);
        Course english = new Course("Advanced English", Subject.ENGLISH);
        Classroom classroom1 = new Classroom("12B");
        Catalogue catalogue = new Catalogue(classroom1);

        classroom1.AddStudent(rares1);
        classroom1.AddStudent(teacher);
        classroom1.AddStudent(rares);
        rares.EnrollIntoCourse(math);
        rares.EnrollIntoCourse(english);
        rares1.EnrollIntoCourse(english);
        rares1.AddGrade(10, english);
        rares1.AddGrade(10, english);
        rares1.AddGrade(10, english);
        rares1.AddGrade(10, english);
        rares1.AddGrade(1, english);
        rares.AddGrade(10, math);
        rares.AddGrade(7, math);
        rares.AddGrade(9, math);
        rares.AddAbsence(new Absence(math));
        rares1.AddAbsence(new Absence(english));

        try
        {
            rares.AddAbsence(new Absence(english));
            rares.AddAbsence(new Absence(math));

        }
        catch (AbsenceException ex)
        {
            Console.WriteLine(ex.Message);
            Throwable.Reset();
        }


        //Console.WriteLine(catalogue.ComputeGpa(rares, math));
        //Console.WriteLine(catalogue.ComputeGpa(rares1, english));
        //try
        //{
        //    Console.WriteLine(catalogue.ComputeGpa(rares, english));
        //} catch (StudentException e)
        //{
        //    Console.WriteLine(e.Message);
        //    Throwable.Reset();
        //}
        //Console.WriteLine(rares);
        //try
        //{
        //    classroom1.AddStudent(rares1);
        //}
        //catch (StudentException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    StudentException.Reset();
        //}
        //try
        //{
        //    classroom1.AddTeacher(teacher);

        //}
        //catch (TeacherException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    TeacherException.Reset();
        //}

        //Console.WriteLine(classroom1.ToString());


        //Console.WriteLine(math);
        //Console.WriteLine(teacher.ToString());


        //try
        //{
        //    teacher.AssignToCourse(english);
        //}
        //catch (TeacherException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    TeacherException.Reset();
        //} catch (Exception ex) 
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //rares.EnrollIntoCourse(math);
        //rares1.EnrollIntoCourse(math);
        //try
        //{
        //    rares.AddGrade(10, math);
        //    rares.AddGrade(7, math);
        //    rares.AddGrade(9, math);
        //    rares.AddGrade(9, english);
        //    rares.AddGrade(1, english);
        //    rares.AddGrade(5, english);

        //}
        //catch (StudentException e)
        //{
        //    Console.WriteLine(e.Message);
        //    StudentException.Reset();
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e.Message);
        //}
        //try
        //{
        //    rares.RemoveGrade(7, math);
        //    rares.RemoveGrade(1, english);
        //}
        //catch (StudentException e)
        //{
        //    Console.WriteLine(e.Message);
        //    StudentException.Reset();
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e.Message);
        //}
        //Console.WriteLine(rares.ToString());
        //Console.WriteLine(math.ToString());
        //Console.WriteLine(teacher.ToString());

    }
}

