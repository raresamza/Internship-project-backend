using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.StudentException
{
    public class StudentAlreadyPartOfClassException : Exception
    {
        public StudentAlreadyPartOfClassException() : base() { }

        public StudentAlreadyPartOfClassException(string message) : base(message) { }

        public StudentAlreadyPartOfClassException(string message, Exception innerException) : base(message, innerException) { }
    }
}
