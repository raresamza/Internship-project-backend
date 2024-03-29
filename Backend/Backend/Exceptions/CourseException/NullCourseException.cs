using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.CourseException;

public class NullCourseException : Exception
{
    public NullCourseException() : base() { }

    public NullCourseException(string message) : base(message) { }

    public NullCourseException(string message, Exception innerException) : base(message, innerException) { }
}
