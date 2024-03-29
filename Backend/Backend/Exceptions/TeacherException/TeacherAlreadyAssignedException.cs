using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.TeacherException;

public class TeacherAlreadyAssignedException : Exception
{
    public TeacherAlreadyAssignedException() : base() { }

    public TeacherAlreadyAssignedException(string message) : base(message) { }

    public TeacherAlreadyAssignedException(string message, Exception innerException) : base(message, innerException) { }
}
