namespace WebApi.Services;


public interface IEmailSenderService
{
    void SendEmail(string email);
    bool IsValidEmail(string email);
}
public class EmailService : IEmailSenderService
{
    public bool IsValidEmail(string email)
    {
        //TODO;
        return true;
    }

    public void SendEmail(string email)
    {
        //TODO;
        Console.WriteLine($"Mail sent: {email}");
    }
}
