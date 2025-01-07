using System.Collections.Generic;

public class ConsumptionStage : EcTransformStage
{
    public override EcPopulationStage CreatePopulationStage()
    {
        return new ConsumptionPopulationStage();
    }
}
