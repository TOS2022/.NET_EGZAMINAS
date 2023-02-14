using System.Data.SQLite;
using System.Globalization;
using RestaurantManager3000.Models;

namespace RestaurantManager3000.Repository;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly SQLiteConnection _connection;

    public RestaurantRepository()
    {
        _connection = new SQLiteConnection("Data Source=.\\database.sqlite; Version = 3; New = True; Compress = True; ");

        try
        {
            _connection.Open();
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to open a connection to the database.");
            throw;
        }
    }

    public List<Table> ReadTables()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * From RestaurantTables";
        var dataReader = command.ExecuteReader();

        var tables = new List<Table>();

        while (dataReader.Read())
        {
            var id = dataReader.GetInt32(0);
            var seats = dataReader.GetInt32(1);
            var occupied = dataReader.GetInt32(2);

            var table = new Table
            {
                Id = id,
                SeatCount = seats,
                Occupied = occupied == 1
            };

            tables.Add(table);
        }

        return tables;
    }

    public void UpdateTable(Table table)
    {
        var command = _connection.CreateCommand();
        var occupiedInt = table.Occupied ? 1 : 0;
        command.CommandText = $"UPDATE RestaurantTables SET Seats = {table.SeatCount}, Occupied = {occupiedInt} WHERE ID = {table.Id};";
        command.ExecuteNonQuery();
    }

    public List<Order> ReadOrders()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * From Orders";
        var dataReader = command.ExecuteReader();

        var orders = new List<Order>();

        while (dataReader.Read())
        {
            var id = dataReader.GetInt32(0);
            var isPaid = dataReader.GetInt32(1);
            string? orderedItems = null;
            try
            {
                orderedItems = dataReader.GetString(2);
            }
            catch (Exception)
            {
                // Field was null, silent catch
            }
            
            var totalAmount = dataReader.GetDouble(3);
            var updateTime = dataReader.GetString(4);
            var tableId = dataReader.GetInt32(5);

            var order = new Order
            {
                Id = id,
                IsPaid = isPaid == 1,
                OrderedItems = ParseItems(orderedItems),
                TotalAmount = totalAmount,
                UpdateTime = DateTime.Parse(updateTime),
                TableId = tableId
            };

            orders.Add(order);
        }

        return orders;
    }

    public void UpdateOrder(Order order)
    {
        var command = _connection.CreateCommand();
        var isPaidInt = order.IsPaid ? 1 : 0;
        var orderedItemsString = ConvertItems(order.OrderedItems);
        orderedItemsString = string.IsNullOrWhiteSpace(orderedItemsString) ? "null" : orderedItemsString;
        command.CommandText = "UPDATE Orders " +
                              $"SET IsPaid = {isPaidInt}, OrderedItems = {orderedItemsString}, TotalAmount = {order.TotalAmount}, " +
                              $"UpdateTime = '{order.UpdateTime.ToString(CultureInfo.InvariantCulture)}', TableId = {order.TableId} " +
                              $"WHERE ID = {order.Id};";
        command.ExecuteNonQuery();
    }

    public void AddOrder(Order order)
    {
        var command = _connection.CreateCommand();
        var isPaidInt = order.IsPaid ? 1 : 0;
        var orderedItemsString = ConvertItems(order.OrderedItems);
        orderedItemsString = string.IsNullOrWhiteSpace(orderedItemsString) ? "null" : orderedItemsString;
        command.CommandText = "INSERT INTO Orders (IsPaid, OrderedItems, TotalAmount, UpdateTime, TableId)" +
                              $"VALUES ({isPaidInt}, {orderedItemsString}, {order.TotalAmount}, '{order.UpdateTime.ToString(CultureInfo.InvariantCulture)}', {order.TableId});";
        command.ExecuteNonQuery();
    }

    public void CloseConnection()
    {
        _connection.Close();
    }

    private static List<Item> ParseItems(string? itemString)
    {
        var parsedItems = new List<Item>();

        if (itemString == null)
        {
            return parsedItems;
        }

        itemString = itemString.TrimEnd(';');

        var splitString = itemString.Split(';');

        for (var i = 0; i < splitString.Length; i += 2)
        {
            var parsedItem = new Item
            {
                Name = splitString[i],
                Price = Convert.ToDouble(splitString[i + 1])
            };

            parsedItems.Add(parsedItem);
        }

        return parsedItems;
    }

    private static string ConvertItems(List<Item> items)
    {
        var convertedString = "";

        foreach (var item in items)
        {
            convertedString += $"{item.Name};{item.Price:0.00};";
        }

        convertedString.TrimEnd(';');

        if (!string.IsNullOrWhiteSpace(convertedString))
        {
            convertedString = convertedString.Insert(0, "\'");
            convertedString += "\'";
        }

        return convertedString;
    }
}