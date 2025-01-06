using System;

public class Function
{
    Func<double, double> lambda;

    public Function(Func<double, double> lambda)
    {
        this.lambda = lambda;
    }

    public double eval(double x)
    {
        return lambda(x);
    }

    public double evalDerivative(double x)
    {
        return (eval(x + 0.001) - eval(x)) / 0.001;
    }
}
