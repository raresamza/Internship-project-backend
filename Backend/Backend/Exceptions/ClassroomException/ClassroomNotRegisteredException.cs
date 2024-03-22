using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.ClassroomException
{
    public class ClassroomNotRegisteredException : Exception
    {
        public ClassroomNotRegisteredException() : base() { }

        public ClassroomNotRegisteredException(string message) : base(message) { }

        public ClassroomNotRegisteredException(string message, Exception innerException) : base(message, innerException) { }
    }
}
