using System.Collections.Generic;
using Unity.VisualScripting;

public class EcTransform
{
    public EcTransformTemplate template;

    public double transformAmount;
    public Dictionary<EcItem, double> inputAmounts;
    public Dictionary<EcItem, double> outputAmounts;
    public Dictionary<EcItem, double> inputDesires;
    public Dictionary<EcItem, double> outputDesires;

    public Dictionary<EcItem, bool> inputShortage;
    public bool hasShortage = false;

    public double LEARNING_RATE = 0.2;

    public EcTransform(EcTransformTemplate template)
    {
        this.template = template;
    }

    public void CalculateAmounts()
    {
        inputAmounts.Clear();
        foreach(EcItem item in template.inputItems)
        {
            inputAmounts.Add(item, template.GetInputAmount(transformAmount, item));
        }
        outputAmounts.Clear();
        foreach(EcItem item in template.outputItems)
        {
            outputAmounts.Add(item, template.GetOutputAmount(transformAmount, item));
        }
    }

    // Use output desires to calculate input desires based on derivatives and shortage.
    public void BackPropagateDesires()
    {
        inputDesires.Clear();

        double totalDodx = 0.0;
        foreach (EcItem outputItem in template.outputItems)
        {
            double dodx = template.GetOutputDerivative(transformAmount, outputItem);
            totalDodx += outputDesires[outputItem] / dodx;
        }

        double totalDidx = 0.0;
        foreach (EcItem inputItem in template.inputItems)
        {
            if (inputShortage[inputItem])
            {
                double didx = template.GetInputDerivative(transformAmount, inputItem);
                totalDidx += didx;
            }
        }

        foreach (EcItem inputItem in template.inputItems)
        {
            if (inputShortage[inputItem])
            {
                double didx = template.GetInputDerivative(transformAmount, inputItem);
                inputDesires[inputItem] = totalDodx * didx / totalDidx;
            }
            else
            {
                inputDesires[inputItem] = 0.0;
            }
        }
    }

    public void Optimize(EcInventory outputInventory)
    {
        CalculateAmounts();
        CalculateShortage(outputInventory);
        if (!hasShortage)
        {
            foreach (EcItem outputItem in template.outputItems)
            {
                double dodx = template.GetOutputDerivative(transformAmount, outputItem);
                transformAmount += LEARNING_RATE * outputDesires[outputItem] / dodx;
            }
        }
        FixOutputBoundaryCondition(outputInventory);
        BackPropagateDesires();
    }

    // Calculate if input items are not enough
    public void CalculateShortage(EcInventory outputInventory)
    {
        inputShortage.Clear();
        hasShortage = false;
        foreach (EcItem inputItem in template.inputItems)
        {
            double shortageAmount = -outputInventory.GetItemAmount(inputItem);
            if (shortageAmount >= -EcTransformTemplate.DELTA)
            {
                inputShortage[inputItem] = true;
                hasShortage = true;
            }
            else
            {
                inputShortage[inputItem] = false;
            }
        }
    }

    // Reduce transform amount if input items are not enough
    public void FixOutputBoundaryCondition(EcInventory outputInventory)
    {
        inputShortage.Clear();
        foreach (EcItem inputItem in template.inputItems)
        {
            inputShortage[inputItem] = false;
        }

        for (int i = 0; i < 5; i++)
        {
            bool hasModified = false;
            foreach (EcItem inputItem in template.inputItems)
            {
                double shortageAmount = -outputInventory.GetItemAmount(inputItem);
                if (shortageAmount > 0.0)
                {
                    hasShortage = true;
                    double didx = template.GetInputDerivative(transformAmount, inputItem);
                    transformAmount -= shortageAmount / didx;
                    inputShortage[inputItem] = true;
                    CalculateAmounts();
                    CalculateShortage(outputInventory);
                }
                else if (shortageAmount >= -EcTransformTemplate.DELTA)
                {
                    inputShortage[inputItem] = true;
                }
                else
                {
                    inputShortage[inputItem] = false;
                }
            }
            if (!hasModified) { break; }
        }
    }

    public string DebugString()
    {
        string str = "Transform amount: " + transformAmount + " || ";
        foreach(EcItem inputItem in template.inputItems)
        {
            str += inputItem.name + ": " + inputAmounts[inputItem].ToShortString() + " @" + inputDesires[inputItem].ToShortString() + " ";
            if (inputShortage[inputItem]) str += "SHORTAGE! ";
        }
        str += "=> ";
        foreach (EcItem outputItem in template.outputItems)
        {
            str += outputItem.name + ": " + outputAmounts[outputItem].ToShortString() + " @" + outputDesires[outputItem].ToShortString() + " ";
        }
        return str + "\n";
    }
}
