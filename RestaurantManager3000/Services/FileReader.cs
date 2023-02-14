using System.Globalization;
using CsvHelper;
using RestaurantManager3000.Models;

namespace RestaurantManager3000.Services;

public class FileReader
{
    public List<Item> ReadDrinks()
    {
        using var reader = new StreamReader(".\\drinks.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Item>().ToList();
        return records;
    }

    public List<Item> ReadFood()
    {
        using var reader = new StreamReader(".\\food.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Item>().ToList();
        return records;
    }
}