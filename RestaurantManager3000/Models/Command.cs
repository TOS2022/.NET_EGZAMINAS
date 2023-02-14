namespace RestaurantManager3000.Models;

public enum Command
{
    CreateOrder,
    AddItemToOrder,
    PayForOrder,
    CreateCheck,
    SendCheck,
    FreeTable,
    None,
    Terminate
}