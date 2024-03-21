using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.ClassroomException
{
    public class ClassroomAlreadyRegisteredException:Exception
    {
        public ClassroomAlreadyRegisteredException() : base() { }

        public ClassroomAlreadyRegisteredException(string message) : base(message) { }

        public ClassroomAlreadyRegisteredException(string message, Exception innerException) : base(message, innerException) { }
    }
}
