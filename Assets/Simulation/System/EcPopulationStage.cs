public class EcPopulationStage
{
    public EcPopulation population;
    public EcStage stage;
    public EcInventory srcInventory;
    public EcInventory dstInventory;

    public EcPopulationStage() { }

    public EcPopulationStage(EcPopulation population, EcStage stage)
    {
        this.population = population;
        this.stage = stage;
    }
}
