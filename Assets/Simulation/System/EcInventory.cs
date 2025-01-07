using System.Collections.Generic;
using Unity.VisualScripting;

public class EcInventory
{
    public List<EcItem> items = new List<EcItem>();
    public Dictionary<EcItem, double> amounts = new Dictionary<EcItem, double>();
    public Dictionary<EcItem, double> desires = new Dictionary<EcItem, double>();

    public void AddItems(List<EcItem> items)
    {
        foreach (EcItem item in items)
        {
            AddItem(item);
        }
    }

    public void AddItem(EcItem item)
    {
        items.Add(item);
        amounts.Add(item, 0.0);
        if (item == EcItem.GetItemById(EcItem.UTILITY))
        {
            desires.Add(item, 1.0);
        }
        else
        {
            desires.Add(item, 0.0);
        }
    }

    public double GetItemAmount(string itemId)
    {
        EcItem item = EcItem.GetItemById(itemId);
        return GetItemAmount(item);
    }

    public double GetItemAmount(EcItem item)
    {
        if (amounts.ContainsKey(item))
        {
            return amounts[item];
        }
        return 0.0;
    }

    public double GetItemDesire(string itemId)
    {
        EcItem item = EcItem.GetItemById(itemId);
        return GetItemDesire(item);
    }

    public double GetItemDesire(EcItem item)
    {
        if (amounts.ContainsKey(item))
        {
            return desires[item];
        }
        return 0.0;
    }

    public string DebugString()
    {
        string str = "";
        foreach (EcItem item in items)
        {
            str += item.name + ": " + amounts[item].ToShortString() + " @" + desires[item].ToShortString() + " | ";
        }
        return str + "\n";
    }
}
