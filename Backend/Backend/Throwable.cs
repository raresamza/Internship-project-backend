using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public interface Throwable
    {
        public static void Reset()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        //public static void LogError()
        //{
        //    Console.ForegroundColor = ConsoleColor.Red;
            
        //    Console.WriteLine("Error logged: TeacherException occurred.");
        //    Console.ForegroundColor = ConsoleColor.White;

        //}

    }
}
