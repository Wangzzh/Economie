using System.Collections.Generic;

public class EcPopulation
{
    public List<EcPopulationStage> populationStages = new();
    public Dictionary<string, double> parameters = new();
    public EcInventory currInventory;
    public EcInventory nextInventory;

    public void AddPopulationStage(EcPopulationStage populationStage)
    {
        populationStages.Add(populationStage);
        populationStage.population = this;
    }

    // Add amounts to inventory after initialized EcSimulation and inventory
    public void AddToCurrentInventory(string itemId, double amount)
    {
        EcItem item = EcItem.GetItemById(itemId);
        if (currInventory.items.Contains(item))
        {
            currInventory.amounts[item] += amount;
        }
    }

    public string DebugString()
    {
        string str = "";
        str += currInventory.DebugString();
        foreach (EcPopulationStage populationStage in populationStages)
        {
            str += populationStage.DebugString();
            str += populationStage.dstInventory.DebugString();
        }
        return str;
    }
}
