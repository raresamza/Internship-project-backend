using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.StudentException
{
    public class StudentAlreadyEnrolledException : Exception
    {
        public StudentAlreadyEnrolledException() : base() { }

        public StudentAlreadyEnrolledException(string message) : base(message) { }

        public StudentAlreadyEnrolledException(string message, Exception innerException) : base(message, innerException) { }
    }
}
