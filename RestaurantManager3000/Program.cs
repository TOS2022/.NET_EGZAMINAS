using System.Net;
using System.Net.Mail;
using RestaurantManager3000.Models;
using RestaurantManager3000.Repository;
using RestaurantManager3000.Services;

// Registering services
var smtpClient = new SmtpClient("smtp.gmail.com")
{
    Port = 587,
    Credentials = new NetworkCredential("RestaurantManager3000@gmail.com", "zxpmtlgcpghognxl"),
    EnableSsl = true
};
const string toEmail = "some_email@gmail.com"; // add email address to send checks to
var emailSender = new EmailSender(smtpClient);
var checkCreator = new CheckCreator(emailSender);
var fileReader = new FileReader();
var restaurantRepository = new RestaurantRepository();
var orderManager = new OrderManager(restaurantRepository);

// Main application cycle
while (true)
{
    Console.Clear();
    var drinks = fileReader.ReadDrinks();
    var food = fileReader.ReadFood();
    var tables = orderManager.GetAllTables();
    var orders = orderManager.GetAllOrders();

    InfoDisplayer.DisplayTables(tables);
    InfoDisplayer.DisplayOrders(orders);
    Console.WriteLine("--- Items ---");
    Console.WriteLine("Food:");
    InfoDisplayer.DisplayItems(food);
    Console.WriteLine("Drinks:");
    InfoDisplayer.DisplayItems(drinks);
    Console.WriteLine();

    var command = InfoDisplayer.GetCommand();

    switch (command)
    {
        case Command.CreateOrder:
        {
            var tableId = InfoDisplayer.SelectTable();
            var table = tables.First(t => t.Id == tableId);
            orderManager.CreateOrder(table);
            break;
        }
        case Command.AddItemToOrder:
        {
            var orderId = InfoDisplayer.SelectOrder();
            var order = orders.First(o => o.Id == orderId);
            var itemName = InfoDisplayer.SelectItem();
            if (string.IsNullOrWhiteSpace(itemName))
            {
                Console.WriteLine("Incorrect name, try anew.");
                break;
            }

            var item = food.FirstOrDefault(f => f.Name == itemName) ?? drinks.First(d => d.Name == itemName);
            orderManager.AddItemToOrder(order, item);
            break;
        }
        case Command.PayForOrder:
        {
            var orderId = InfoDisplayer.SelectOrder();
            var order = orders.First(o => o.Id == orderId);
            var amount = InfoDisplayer.SelectAmountToPay();
            orderManager.PayForOrder(order);
            break;
        }
        case Command.CreateCheck:
        {
            var orderId = InfoDisplayer.SelectOrder();
            var order = orders.First(o => o.Id == orderId);
            var check = checkCreator.CreateCheck(order);
            Console.WriteLine(check);
            Console.WriteLine("Would you like to send this check? y/n");
            var key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (key == 'Y' || key == 'y')
            {
                checkCreator.SendCheck(check, $"Check_for_order_{order.Id}.txt", toEmail, "RestaurantManager3000@gmail.com");
            }
            break;
        }
        case Command.SendCheck:
        {
            var orderId = InfoDisplayer.SelectOrder();
            var checkName = $"Check_for_order_{orderId}.txt";
            var check = checkCreator.ReadCheck(checkName);
            checkCreator.SendCheck(check, checkName, toEmail, "RestaurantManager3000@gmail.com");
            break;
        }
        case Command.FreeTable:
        {
            var tableId = InfoDisplayer.SelectTable();
            var table = tables.First(t => t.Id == tableId);
            orderManager.FreeTable(table);
            break;
        }
    }

    if (command == Command.Terminate)
    {
        break;
    }

    Console.WriteLine("Executed. Press any key to refresh.");
    Console.ReadKey();
}

Console.WriteLine("Terminating program...");
restaurantRepository.CloseConnection();
smtpClient.Dispose();
Console.ReadKey();