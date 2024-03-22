using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.AbsenceException
{
    internal class DuplicateAbsenceException : Exception
    {
        public DuplicateAbsenceException() : base() { }

        public DuplicateAbsenceException(string message) : base(message) { }

        public DuplicateAbsenceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
