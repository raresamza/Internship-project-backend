using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Exceptions.HomeworkException;

public class NullHomeworkException : Exception
{
    public NullHomeworkException(): base() {}
    public NullHomeworkException(string message) : base(message) {}
    public NullHomeworkException(string message, Exception innerException) : base(message, innerException) {}
}
