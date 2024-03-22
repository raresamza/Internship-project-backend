using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.ClassroomException
{
    public class NullClassroomException : Exception
    {
        public NullClassroomException() : base() { }

        public NullClassroomException(string message) : base(message) { }

        public NullClassroomException(string message, Exception innerException) : base(message, innerException) { }
    }
}
