using RestaurantManager3000.Models;
using RestaurantManager3000.Repository;

namespace RestaurantManager3000.Services;

public class OrderManager
{
    private readonly IRestaurantRepository _restaurantRepository;

    public OrderManager(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public Order? CreateOrder(Table table)
    {
        var dbTables = _restaurantRepository.ReadTables();
        var dbTable = dbTables.First(t => t.Id == table.Id);

        if (dbTable.Occupied)
        {
            Console.WriteLine("Table already occupied. Try again.");
            return null;
        }

        var order = new Order
        {
            IsPaid = false,
            OrderedItems = new List<Item>(),
            TotalAmount = 0.0,
            UpdateTime = DateTime.UtcNow,
            TableId = table.Id
        };

        _restaurantRepository.AddOrder(order);
        table.Occupied = true;
        _restaurantRepository.UpdateTable(table);
        return order;
    }

    public void AddItemToOrder(Order order, Item item)
    {
        order.TotalAmount += item.Price;
        order.OrderedItems.Add(item);
        _restaurantRepository.UpdateOrder(order);
    }

    public void PayForOrder(Order order)
    {
        order.IsPaid = true;
        _restaurantRepository.UpdateOrder(order);
    }

    public void FreeTable(Table table)
    {
        table.Occupied = false;
        _restaurantRepository.UpdateTable(table);
    }

    public List<Order> GetAllOrders()
    {
        var orders = _restaurantRepository.ReadOrders();
        return orders;
    }

    public List<Table> GetAllTables()
    {
        var tables = _restaurantRepository.ReadTables();
        return tables;
    }
}