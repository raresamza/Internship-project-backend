using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Exceptions;

namespace Backend.Exceptions.Placeholders
{
    public class ClassroomException : Exception, Throwable
    {
        public ClassroomException(string message) : base(message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public static void LogError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error logged: ClassroomException occurred.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //public static void Reset()
        //{
        //    Console.ForegroundColor = ConsoleColor.White;
        //}
    }
}
