using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Abstractions;

public interface IMailService
{
    Task<bool> SendEmail(string emailAddress);
    bool IsValidEmail(string email);
    Task<bool> SendSimpleEmailAsync(string email, string subject, string body);

    Task<bool> SendGradePdfAsync(string email, Stream pdfStream);
}
