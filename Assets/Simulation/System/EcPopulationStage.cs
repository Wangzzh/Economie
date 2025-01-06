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

    public string DebugString(bool withInventory = false)
    {
        string str = "";
        if (withInventory)
        {
            str += srcInventory.DebugString();
        }
        str += this.GetType().Name + "\n";
        if (withInventory)
        {
            str += dstInventory.DebugString();
        }
        return str;
    }
}
