using System.Collections.Generic;
using System.Linq;

public class EcSimulation
{
    public List<EcPopulation> populations = new();
    public List<EcStage> stages = new();
    public List<EcItem> items;

    public int optimizationIterations = 20;

    bool initialized = false;
    public int numSteps = 0;

    public void AddPopulation(EcPopulation population)
    {
        populations.Add(population);
        initialized = false;
    }

    public void AddStage(EcStage stage)
    {
        stages.Add(stage);
        initialized = false;
    }

    public void Initialize()
    {
        // Get related items from stages
        HashSet<EcItem> itemsSet = new();
        foreach (EcStage stage in stages)
        {
            HashSet<EcItem> relatedItems = stage.ListAllRelatedItems();
            foreach(EcItem item in relatedItems)
            {
                itemsSet.Add(item);
            }
        }
        items = itemsSet.ToList();

        // Initialize population and population stages
        foreach(EcPopulation population in populations)
        {
            EcInventory inventory = new();
            inventory.AddItems(items);
            population.currInventory = inventory;

            foreach(EcStage stage in stages)
            {
                EcPopulationStage populationStage = stage.CreatePopulationStage();
                stage.AddPopulationStage(populationStage);
                population.AddPopulationStage(populationStage);
                populationStage.srcInventory = inventory;
                inventory = new EcInventory();
                inventory.AddItems(items);
                populationStage.dstInventory = inventory;
            }

            population.nextInventory = inventory;
        }

        initialized = true;
    }

    public void Optimize()
    {
        for (int i = 0; i < optimizationIterations; i++)
        {
            foreach(EcStage stage in stages)
            {
                stage.Optimize();
            }
        }
    }

    public void StepInventory()
    {
        foreach (EcPopulation population in populations)
        {
            foreach (EcItem item in items)
            {
                population.currInventory.amounts[item] = population.nextInventory.amounts[item];
            }
        }
    }

    public void Step()
    {
        Optimize();
        StepInventory();
        Optimize();
        numSteps += 1;
    }

    public string DebugString()
    {
        string str = "";
        str += "Num steps: " + numSteps + "\n";

        str += "\n";
        int populationId = 0;
        foreach(EcPopulation population in populations)
        {
            str += "Population #" + populationId + ":\n";
            str += population.DebugString();
            str += "\n";
        }
        return str;
    }
}
