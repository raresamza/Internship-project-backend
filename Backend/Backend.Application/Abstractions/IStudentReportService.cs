using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public interface IStudentReportService
{
    Task<byte[]> GenerateReportAsync(int studentId);
}
