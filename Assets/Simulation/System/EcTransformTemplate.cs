using System;
using System.Collections.Generic;

public class EcTransformTemplate
{
    public List<EcItem> inputItems;
    public List<EcItem> outputItems;
    public double minTransformAmount = 0.0;
    public double maxTransformAmount = 1000.0;

    public static double DELTA = 0.001;

    public virtual double GetInputAmount(double transformAmount, EcItem item)
    {
        return 0.0;
    }

    public virtual double GetOutputAmount(double transformAmount, EcItem item)
    {
        return 0.0;
    }

    // d(inputAmount)/d(transformAmount)
    public double GetInputDerivative(double transformAmount, EcItem item)
    {
        return (GetInputAmount(transformAmount + DELTA, item) - GetInputAmount(transformAmount, item)) / DELTA;
    }

    // d(outputAmount)/d(transformAmount)
    public double GetOutputDerivative(double transformAmount, EcItem item)
    {
        return (GetOutputAmount(transformAmount + DELTA, item) - GetOutputAmount(transformAmount, item)) / DELTA;
    }

    public virtual EcTransform CreateTransform()
    {
        return new EcTransform(this);
    }
}
