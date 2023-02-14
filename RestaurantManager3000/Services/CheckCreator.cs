using System.Net.Mail;
using RestaurantManager3000.Models;

namespace RestaurantManager3000.Services;

public class CheckCreator
{
    private readonly IEmailSender _emailSender;

    public CheckCreator(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public string CreateCheck(Order order)
    {
        var paidString = order.IsPaid ? "Paid" : "Not paid";
        var checkString =
            $"Order id: {order.Id}, Is paid: {paidString}, Ordered items: {order.ListOrderedItems()}, Total amount: {order.TotalAmount:0.00}, Order update time: {order.UpdateTime}, " +
            $"Table id: {order.TableId}, Check creation time: {DateTime.UtcNow}";

        var fileName = $"Check_for_order_{order.Id}.txt";

        using var writer = new StreamWriter(fileName);
        writer.Write(checkString);

        return checkString;
    }

    public string ReadCheck(string fileName)
    {
        var fileText = File.ReadAllText(fileName);
        return fileText;
    }

    public void SendCheck(string checkString, string fileName, string sendTo, string sendFrom)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(sendFrom),
            Subject = "Restaurant check",
            Body = $"Check: {checkString}",
            IsBodyHtml = true
        };

        mailMessage.To.Add(sendTo);
        var attachment = new Attachment(fileName);
        mailMessage.Attachments.Add(attachment);

        try
        {
            _emailSender.SendEmail(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while trying to send an email. Exception: {e.Message}");
            Console.ReadKey();
        }
    }
}