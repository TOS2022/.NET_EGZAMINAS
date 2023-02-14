using System.Net.Mail;

namespace RestaurantManager3000.Services;

public interface IEmailSender
{
    void SendEmail(MailMessage mailMessage);
}