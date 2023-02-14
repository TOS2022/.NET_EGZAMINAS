using System.Net.Mail;
using Moq;
using RestaurantManager3000.Models;
using RestaurantManager3000.Services;
using Xunit;

namespace RestaurantManager3000.UnitTests;

public class CheckCreatorTests
{
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly CheckCreator _checkCreator;

    public CheckCreatorTests()
    {
        _emailSenderMock = new Mock<IEmailSender>();
        _emailSenderMock
            .Setup(s => s.SendEmail(It.IsAny<MailMessage>()))
            .Verifiable();

        _checkCreator = new CheckCreator(_emailSenderMock.Object);
    }

    [Fact]
    public void SendCheckShouldSendEmail()
    {
        var order = new Order
        {
            Id = 0,
            IsPaid = true,
            OrderedItems = new List<Item>
            {
                new Item
                {
                    Name = "nameTest",
                    Price = 0.0
                }
            },
            TableId = 0,
            TotalAmount = 0.0,
            UpdateTime = DateTime.UtcNow
        };

        var check = _checkCreator.CreateCheck(order);
        _checkCreator.SendCheck(check, $"Check_for_order_{order.Id}.txt", "to@email.com", "from@emgil.com");

        _emailSenderMock.Verify(s => s.SendEmail(It.IsAny<MailMessage>()), Times.Once);
    }
}