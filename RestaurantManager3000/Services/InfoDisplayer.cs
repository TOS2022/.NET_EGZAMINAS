using RestaurantManager3000.Models;

namespace RestaurantManager3000.Services;

public static class InfoDisplayer
{
    public static void DisplayTables(List<Table> tables)
    {
        Console.WriteLine("--- Tables ---");
        Console.WriteLine("Table id, seat count, is occupied");

        foreach (var table in tables)
        {
            var occupiedString = table.Occupied ? "Occupied" : "Free";
            Console.WriteLine($"{table.Id}, {table.SeatCount}, {occupiedString}");
        }
    }

    public static void DisplayOrders(List<Order> orders)
    {
        Console.WriteLine("--- Orders ---");
        Console.WriteLine("Order id, is paid, ordered items, total amount, update time, table id");

        foreach (var order in orders)
        {
            var paidString = order.IsPaid ? "Paid" : "Not paid";
            Console.WriteLine($"{order.Id}, {paidString}, {order.ListOrderedItems()}, {order.TotalAmount:0.00}, {order.UpdateTime}, {order.TableId}");
        }
    }

    public static void DisplayItems(List<Item> items)
    {
        Console.WriteLine("Name, price");

        foreach (var item in items)
        {
            Console.WriteLine($"{item.Name}, {item.Price:0.00}");
        }
    }

    public static Command GetCommand()
    {
        Console.WriteLine();
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("Create [O]rder, [A]dd item to order, [P]ay for order, [C]reate check, [S]end check, [F]ree table, [T]erminate program, [Anything else] cancel");

        var input = Console.ReadKey().KeyChar;

        switch (input)
        {
            case 'O':
            case 'o':
                return Command.CreateOrder;
            case 'A':
            case 'a':
                return Command.AddItemToOrder;
            case 'P':
            case 'p':
                return Command.PayForOrder;
            case 'C':
            case 'c':
                return Command.CreateCheck;
            case 'S':
            case 's':
                return Command.SendCheck;
            case 'F':
            case 'f':
                return Command.FreeTable;
            case 'T':
            case 't':
                return Command.Terminate;
            default:
                return Command.None;
        }
    }

    public static int SelectTable()
    {
        Console.WriteLine();
        Console.WriteLine("Please select a table number:");
        var input = Console.ReadLine();
        var tableNumber = Convert.ToInt32(input);
        return tableNumber;
    }

    public static string? SelectItem()
    {
        Console.WriteLine();
        Console.WriteLine("Please select a product (type its name correctly):");
        var input = Console.ReadLine();
        return input;
    }

    public static int SelectOrder()
    {
        Console.WriteLine();
        Console.WriteLine("Please select an order number:");
        var input = Console.ReadLine();
        var orderNumber = Convert.ToInt32(input);
        return orderNumber;
    }

    public static double SelectAmountToPay()
    {
        Console.WriteLine();
        Console.WriteLine("Please select amount to be paid:");
        var input = Console.ReadLine();
        var amount = Convert.ToDouble(input);
        return amount;
    }
}