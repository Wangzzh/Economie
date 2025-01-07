using System.Collections.Generic;
using UnityEngine;

public class EcTransformStage : EcIndividualStage
{
    public List<EcTransformTemplate> templates;

    public override EcPopulationStage CreatePopulationStage()
    {
        EcTransformPopulationStage populationStage = new();
        foreach (EcTransformTemplate template in templates)
        {
            populationStage.transforms.Add(template.CreateTransform());
        }
        return populationStage;
    }

    public override HashSet<EcItem> ListAllRelatedItems()
    {
        HashSet<EcItem> items = new HashSet<EcItem>();
        foreach (EcTransformTemplate template in templates)
        {
            foreach (EcItem item in template.inputItems)
            {
                items.Add(item);
            }
            foreach (EcItem item in template.outputItems)
            {
                items.Add(item);
            }
        }
        return items;
    }

    public override string DebugString()
    {
        string str = "";
        str += this.GetType().Name + ":\n";
        foreach (EcTransformTemplate template in templates)
        {
            str += "- " + template.DebugString();
        }
        return str;
    }
}
