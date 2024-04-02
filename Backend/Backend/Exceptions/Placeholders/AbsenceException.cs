using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.Placeholders;

public class AbsenceException : Exception, Throwable
{

    public AbsenceException(string message) : base(message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    public static void LogError()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error logged: AbsenceException occurred.");
        Console.ForegroundColor = ConsoleColor.White;
    }
}
