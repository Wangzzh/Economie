using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConsumptionPopulationStage : EcPopulationStage
{
    public Dictionary<ConsumptionType, bool> enabled;
    public Dictionary<ConsumptionType, double> consumedAmount = new(); // Scaled with population
    public Dictionary<EcItem, double> directDesire = new();
    public static double DELTA = 0.0001;
    public static double LEARNING_RATE = 0.3;

    public override void Initialize() 
    {
        enabled = (stage as ConsumptionStage).enabled;
        foreach (ConsumptionType type in enabled.Keys)
        {
            consumedAmount.Add(type, 0.0);
        }
    }

    public override void Optimize() 
    {
        CalculateDirectDesire();
        CalculateConsumption();
        UpdateInventory();
        PropagateDesire();
    }

    public void PropagateDesire()
    {
        foreach (EcItem item in srcInventory.items)
        {
            srcInventory.desires[item] = dstInventory.desires[item];
        }
        if (enabled.ContainsKey(ConsumptionType.FOOD) && enabled[ConsumptionType.FOOD])
        {
            srcInventory.desires[EcItem.GetItemById(EcItem.FOOD)] = Math.Max(dstInventory.GetItemDesire(EcItem.FOOD), directDesire[EcItem.GetItemById(EcItem.FOOD)]);
        }
    }

    public void CalculateDirectDesire()
    {
        directDesire.Clear();
        if (enabled.ContainsKey(ConsumptionType.FOOD) && enabled[ConsumptionType.FOOD])
        {
            directDesire.Add(EcItem.GetItemById(EcItem.FOOD), EvaluateDesire(ConsumptionType.FOOD));
        }
    }

    public void CalculateConsumption()
    {
        if (enabled.ContainsKey(ConsumptionType.FOOD) && enabled[ConsumptionType.FOOD])
        {
            Debug.Log("Direct desire: " + directDesire[EcItem.GetItemById(EcItem.FOOD)]);
            Debug.Log("Dst desire: " + dstInventory.GetItemDesire(EcItem.FOOD));
            var newConsumedAmount = consumedAmount[ConsumptionType.FOOD] + LEARNING_RATE * (directDesire[EcItem.GetItemById(EcItem.FOOD)] - dstInventory.GetItemDesire(EcItem.FOOD));
            newConsumedAmount = Math.Clamp(newConsumedAmount, 0.0, srcInventory.GetItemAmount(EcItem.FOOD));
            consumedAmount[ConsumptionType.FOOD] = newConsumedAmount;
        }
    }

    public void UpdateInventory()
    {
        foreach (EcItem item in srcInventory.items)
        {
            dstInventory.amounts[item] = srcInventory.amounts[item];
        }
        if (enabled.ContainsKey(ConsumptionType.FOOD) && enabled[ConsumptionType.FOOD])
        {
            dstInventory.amounts[EcItem.GetItemById(EcItem.FOOD)] -= consumedAmount[ConsumptionType.FOOD];
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
        foreach (ConsumptionType type in enabled.Keys)
        {
            if ((stage as ConsumptionStage).enabled[type])
            {
                str += type.ToString() + ": " + consumedAmount[type] + " ->" + EvaluateUtility(type).ToShortString() + " | ";
            }
        }
        str += "|| ";
        foreach (EcItem item in directDesire.Keys)
        {
            str += item.name + " @" + directDesire[item].ToShortString() + " | ";
        }
        str += "\n";
        if (withInventory)
        {
            str += dstInventory.DebugString();
        }
        return str;
    }

    public double EvaluateUtility(ConsumptionType type, double delta = 0.0)
    {
        switch (type)
        {
            case ConsumptionType.FOOD:
                return Math.Log(consumedAmount[type] / srcInventory.GetItemAmount(EcItem.POPULATION) + DELTA + delta);
            default:
                return 0.0;
        }
    }

    public double EvaluateDesire(ConsumptionType type)
    {
        double derivative = (EvaluateUtility(type, DELTA) - EvaluateUtility(type)) / DELTA;
        derivative = Math.Clamp(derivative, -10, 10);
        Debug.Log("Derivative: " + derivative);
        return derivative;
    }

}