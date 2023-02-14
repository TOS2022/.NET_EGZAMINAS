using RestaurantManager3000.Models;

namespace RestaurantManager3000.Repository;

public interface IRestaurantRepository
{
    List<Table> ReadTables();
    void UpdateTable(Table table);
    List<Order> ReadOrders();
    void UpdateOrder(Order order);
    void AddOrder(Order order);
    void CloseConnection();
}