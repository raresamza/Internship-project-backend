using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.StudentException;

public class StudentNotEnrolledException : Exception
{
    public StudentNotEnrolledException() : base() { }

    public StudentNotEnrolledException(string message) : base(message) { }

    public StudentNotEnrolledException(string message, Exception innerException) : base(message, innerException) { }
}
