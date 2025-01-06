using System.Collections.Generic;

public partial class EcItem
{
    public static Dictionary<string, EcItem> itemsDictionary = new();

    public static string FOOD = "Food";
    public static string GOLD = "Gold";
    public static string LABOR = "Labor";
    public static string LAND_OWNERSHIP = "Land Ownership";
    public static string LAND_USAGE = "Land Usage";
    public static string POPULATION = "Population";

    public string name = "Unknown Item";

    public static EcItem GetItemById(string id)
    {
        if (itemsDictionary.ContainsKey(id)) return itemsDictionary[id];
        
        EcItem item = new EcItem();
        item.name = id.ToLower();
        itemsDictionary.Add(id, item);
        return itemsDictionary[id];
    }
}
