using System.Collections.Generic;

public class ConsumptionStage : EcIndividualStage
{
    public Dictionary<ConsumptionType, bool> enabled = new();

    public override EcPopulationStage CreatePopulationStage()
    {
        return new ConsumptionPopulationStage();
    }

    public override void Initialize() { }

    public override HashSet<EcItem> ListAllRelatedItems()
    {
        HashSet<EcItem> items = new HashSet<EcItem>();
        if (enabled[ConsumptionType.FOOD])
        {
            items.Add(EcItem.GetItemById(EcItem.FOOD));
        }
        return items;
    }

    public override string DebugString()
    {
        string str = "";
        str += this.GetType().Name + ": ";
        foreach(ConsumptionType type in enabled.Keys) {
            if (enabled[type])
            {
                str += type.ToString() + " ";
            }
        }
        return str + "\n";
    }
}
