using System.Collections.Generic;

public class EcPopulation
{
    public List<EcPopulationStage> populationStages = new();
    public EcInventory currInventory;
    public EcInventory nextInventory;

    public void AddPopulationStage(EcPopulationStage populationStage)
    {
        populationStages.Add(populationStage);
        populationStage.population = this;
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
