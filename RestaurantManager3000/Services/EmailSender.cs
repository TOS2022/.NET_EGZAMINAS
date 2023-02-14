using System.Net.Mail;

namespace RestaurantManager3000.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailSender(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }

    public void SendEmail(MailMessage mailMessage)
    {
        _smtpClient.Send(mailMessage);
    }
}