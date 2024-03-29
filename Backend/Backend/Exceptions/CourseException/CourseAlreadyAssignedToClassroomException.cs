using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.CourseException;

public class CourseAlreadyAssignedToClassroomException : Exception
{
    public CourseAlreadyAssignedToClassroomException() : base() { }

    public CourseAlreadyAssignedToClassroomException(string message) : base(message) { }

    public CourseAlreadyAssignedToClassroomException(string message, Exception innerException) : base(message, innerException) { }
}
