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
}
