using System;
using UnityEngine;

public class Test : MonoBehaviour
{

    Function func = new Function((x) => Math.Log(x));
    double x = 3.0;

    void Update()
    {
        var value = func.eval(x);
        var derivative = func.evalDerivative(x);

        Debug.Log("x: " + x);
        Debug.Log("Value: " + value);
        Debug.Log("Deriv: " + derivative);

        this.x -= 0.2 * value / derivative;
    }
}
