using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class TeacherException : Exception
    {
        //public TeacherException() : base()
        //{
        //}

        public TeacherException(string message) : base(message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void Reset()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        //public TeacherException(string message, Exception innerException) : base(message, innerException)
        //{
        //}

        public static void LogError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error logged: TeacherException occurred.");
            Console.ForegroundColor = ConsoleColor.White;

        }

        //public void NotifyAdmin()
        //{
        //    // Dummy method to notify admin
        //    Console.WriteLine("Admin notified: TeacherException occurred.");
        //}
    }
}
