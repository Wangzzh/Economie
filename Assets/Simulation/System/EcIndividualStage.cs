public class EcIndividualStage : EcStage
{
    public override void Optimize() {
        foreach(EcPopulationStage ecPopulationStage in this.populationStages)
        {
            ecPopulationStage.Optimize();
        }
    }
}
