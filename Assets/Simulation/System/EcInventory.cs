using System.Collections.Generic;

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
        desires.Add(item, 0.0);
    }

    public string DebugString()
    {
        string str = "";
        foreach (EcItem item in items)
        {
            str += item.ToString() + ": " + amounts[item] + " @" + desires[item] + " | ";
        }
        return str + "\n";
    }
}
