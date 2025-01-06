using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    // Create population stages and inventories given populations and stages
    public void Initialize()
    {
        // Get related items from stages
        HashSet<EcItem> itemsSet = new()
        {
            EcItem.GetItemById(EcItem.LABOR),
            EcItem.GetItemById(EcItem.LAND_USAGE),
            EcItem.GetItemById(EcItem.LAND_OWNERSHIP),
            EcItem.GetItemById(EcItem.POPULATION),
        };
        foreach (EcStage stage in stages)
        {
            stage.Initialize();
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
                populationStage.Initialize();
            }

            population.nextInventory = inventory;
        }

        initialized = true;
    }

    public void Optimize(int iterations = 1)
    {
        for (int i = 0; i < iterations; i++)
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
                population.nextInventory.desires[item] = population.currInventory.desires[item];
            }
        }
    }

    public void Step()
    {
        Optimize(optimizationIterations);
        StepInventory();
        Optimize(optimizationIterations);
        numSteps += 1;
    }

    public string DebugString()
    {
        string str = "";
        str += "Num steps: " + numSteps + "\n";
        str += "\n";

        int stageId = 0;
        foreach(EcStage stage in stages)
        {
            str += "Stage: #" + stageId + ": " + stage.DebugString() + "\n";
        }
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
