using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.AbsenceException;

internal class InvalidAbsenceException : Exception
{
    public InvalidAbsenceException() : base() { }

    public InvalidAbsenceException(string message) : base(message) { }

    public InvalidAbsenceException(string message, Exception innerException) : base(message, innerException) { }
}
