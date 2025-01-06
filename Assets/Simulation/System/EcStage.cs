using System.Collections.Generic;

public class EcStage
{
    public List<EcPopulationStage> populationStages = new();

    public void AddPopulationStage(EcPopulationStage populationStage)
    {
        populationStages.Add(populationStage);
        populationStage.stage = this;
    }

    public virtual void Initialize() { }

    public virtual EcPopulationStage CreatePopulationStage()
    {
        return new EcPopulationStage();
    }

    public virtual void Optimize() { }

    public virtual HashSet<EcItem> ListAllRelatedItems()
    {
        return new HashSet<EcItem>();
    }

    public virtual string DebugString()
    {
        string str = "";
        str += this.GetType().Name + "\n";
        return str;
    }
}
