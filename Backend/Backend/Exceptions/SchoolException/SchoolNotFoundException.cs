using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Exceptions.SchoolException;

public class SchoolNotFoundException : Exception
{
    public SchoolNotFoundException() : base() { }

    public SchoolNotFoundException(string message) : base(message) { }

    public SchoolNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
