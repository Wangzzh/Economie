using System;

public class FoodConsumptionTemplate : EcTransformTemplate
{
    public FoodConsumptionTemplate()
    {
        inputItems = new();
        outputItems = new();
        inputItems.Add(EcItem.GetItemById(EcItem.FOOD));
        outputItems.Add(EcItem.GetItemById(EcItem.UTILITY));
    }

    public override double GetInputAmount(double transformAmount, EcItem item)
    {
        return transformAmount;
    }

    public override double GetOutputAmount(double transformAmount, EcItem item)
    {
        return Math.Log(transformAmount + 0.001);
    }
}
