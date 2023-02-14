namespace RestaurantManager3000.Models;

public class Order
{
    public int Id { get; set; }
    public bool IsPaid { get; set; }
    public double TotalAmount { get; set; }
    public DateTime UpdateTime { get; set; }
    public List<Item> OrderedItems { get; set; }
    public int TableId { get; set; }

    public string ListOrderedItems()
    {
        var orderedItemsString = "";

        foreach (var orderedItem in OrderedItems)
        {
            orderedItemsString += $"{orderedItem.Name}: {orderedItem.Price:0.00}; ";
        }

        return orderedItemsString;
    }
}