using Backend;

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
        classroom1.addStudent(rares1);
        classroom1.addTeacher(teacher);
        classroom1.addStudent(rares);

        //try
        //{
        //    classroom1.addStudent(rares1);
        //}
        //catch (StudentException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    StudentException.Reset();
        //}
        //try
        //{
        //    classroom1.addTeacher(teacher);

        //}
        //catch (TeacherException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //    TeacherException.Reset();
        //}

        Console.WriteLine(classroom1.ToString());


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
        //rares.enrollIntoCourse(math);
        //rares1.enrollIntoCourse(math);
        //try
        //{
        //    rares.addGrade(10, math);
        //    rares.addGrade(7, math);
        //    rares.addGrade(9, math);
        //    rares.addGrade(9, english);
        //    rares.addGrade(1, english);
        //    rares.addGrade(5, english);

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
        //    rares.removeGrade(7, math);
        //    rares.removeGrade(1, english);
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

