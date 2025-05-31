using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Abstractions;

namespace WebApi.Services;




public class MailService:IMailService
{
    public async Task<bool> SendEmail(string emailAddress)
    {
        if (!IsValidEmail(emailAddress))
        {
            //replace cw with error
            Console.WriteLine("Please provide a valid e-mail address.");
            return await Task.FromResult(false);
        }
        else
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("raresamza@gmail.com");
                    mailMessage.To.Add(emailAddress);

                    mailMessage.Subject = "Thanks for subbing to our newsletter";
                    mailMessage.Body = "Thank you for subscribing to our newsletter.\nWe are delighted by the fact that you are interested in our products and want to stay up to date with everything we do\n.Thank you!";

                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Credentials = new NetworkCredential("raresamza@gmail.com", "qygs zndu mbqh sjrs");
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
                return false;
            }
        }
    }

    public async Task<bool> SendGradePdfAsync(string email, Stream pdfStream)
    {
        try
        {
            using var mail = new MailMessage();
            mail.From = new MailAddress("raresamza@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Student Grade Chart";
            mail.Body = "Attached is the student grade chart in PDF format.";

            pdfStream.Position = 0;
            var attachment = new Attachment(pdfStream, "grades.pdf", "application/pdf");
            mail.Attachments.Add(attachment);

            using var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("raresamza@gmail.com", "qygs zndu mbqh sjrs"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool IsValidEmail(string email)
    {
        try
        {
            MailAddress mail = new MailAddress(email);
            return mail.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendSimpleEmailAsync(string email, string subject, string body)
    {
        try
        {
            var mail = new MailMessage("raresamza@gmail.com", email, subject, body);
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("raresamza@gmail.com", "qygs zndu mbqh sjrs"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email failed: {ex.Message}");
            return false;
        }
    }

}
