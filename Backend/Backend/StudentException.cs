using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class StudentException : Exception
    {
        //public StudentException() : base()
        //{
        //}

        public StudentException(string message) : base(message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

        }

        //public StudentException(string message, Exception innerException) : base(message, innerException)
        //{
        //}

        public static void LogError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error logged: StudentException occurred.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Reset()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
