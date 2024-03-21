using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions;

namespace Backend.Exceptions.Placeholders
{
    public class CourseException : Exception, Throwable
    {
        public CourseException(string message) : base(message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

        }
        public static void LogError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error logged: StudentException occurred.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //public static void Reset()
        //{
        //    Console.ForegroundColor = ConsoleColor.White;
        //}
    }
}
