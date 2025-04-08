using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.TeacherException;

public class TeacherNotFoundException : Exception
{
    public TeacherNotFoundException() : base("Teacher Not Found") { }

    public TeacherNotFoundException(string message) : base(message) { }

    public TeacherNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
