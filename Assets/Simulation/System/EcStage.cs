using System.Collections.Generic;

public class EcStage
{
    public List<EcPopulationStage> populationStages = new();

    public void AddPopulationStage(EcPopulationStage populationStage)
    {
        populationStages.Add(populationStage);
        populationStage.stage = this;
    }

    public EcPopulationStage CreatePopulationStage()
    {
        return new EcPopulationStage();
    }

    public void Optimize()
    {
        // Do nothing
    }

    public HashSet<EcItem> ListAllRelatedItems()
    {
        return new HashSet<EcItem>();
    }
}
