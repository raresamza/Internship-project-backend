using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Exceptions.StudentException;

public class StudentAbsenceException : Exception
{
    public StudentAbsenceException() : base() { }

    public StudentAbsenceException(string message) : base(message) { }

    public StudentAbsenceException(string message, Exception innerException) : base(message, innerException) { }
}
