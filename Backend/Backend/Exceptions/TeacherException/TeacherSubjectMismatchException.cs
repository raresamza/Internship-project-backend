using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Exceptions.TeacherException
{
    public class TeacherSubjectMismatchException : Exception
    {
        public TeacherSubjectMismatchException() : base() { }

        public TeacherSubjectMismatchException(string message) : base(message) { }

        public TeacherSubjectMismatchException(string message, Exception innerException) : base(message, innerException) { }
    }
}
