using System;
using System.Collections.Generic;

public class EcTransformPopulationStage : EcPopulationStage
{
    public List<EcTransform> transforms = new();

    public override void Optimize()
    {
        foreach (var transform in transforms)
        {
            transform.CopyOutputDesires(dstInventory);
            transform.Optimize(dstInventory);
        }
        UpdateOutputInventory();
        BackPropagateInputDesires();
    }

    public void UpdateOutputInventory()
    {
        foreach (EcItem item in srcInventory.items)
        {
            dstInventory.amounts[item] = srcInventory.amounts[item];
        }
        foreach (EcTransform transform in transforms) 
        {
            foreach(EcItem inputItem in transform.inputAmounts.Keys)
            {
                dstInventory.amounts[inputItem] -= transform.inputAmounts[inputItem];
            }
            foreach(EcItem outputItem in transform.outputAmounts.Keys)
            {
                dstInventory.amounts[outputItem] += transform.outputAmounts[outputItem];
            }
        }
    }

    public void BackPropagateInputDesires()
    {
        foreach (EcItem item in srcInventory.items)
        {
            srcInventory.desires[item] = dstInventory.desires[item];
        }
        foreach (EcTransform transform in transforms)
        {
            foreach (EcItem inputItem in transform.inputDesires.Keys)
            {
                srcInventory.desires[inputItem] = Math.Max(dstInventory.GetItemDesire(inputItem), transform.inputDesires[inputItem]);
            }
        }
    }

    public override string DebugString(bool withInventory = false)
    {
        string str = "";
        if (withInventory)
        {
            str += srcInventory.DebugString();
        }
        str += this.GetType().Name + ": ";
        foreach (EcTransform transform in transforms)
        {
            str += transform.DebugString();
        }
        if (withInventory)
        {
            str += dstInventory.DebugString();
        }
        return str;
    }
}
