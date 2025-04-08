using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Exceptions.HomeworkException;

public class HomeworkNotFoundException : Exception
{


    public HomeworkNotFoundException() : base() { }

    public HomeworkNotFoundException(string message) : base(message) { }

    public HomeworkNotFoundException(string message, Exception innerException) : base(message, innerException) { }

}
