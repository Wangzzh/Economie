using UnityEngine;

public class TestSimulation : MonoBehaviour
{
    EcSimulation simulation;
    public TMPro.TMP_Text debugText;

    EcPopulation population = new EcPopulation();
    EcTransformStage consumptionStage = new();

    void Start()
    {
        Debug.Log("Simulation initializing...");
        simulation = new EcSimulation();
        
        simulation.AddPopulation(population);

        consumptionStage.templates = new()
        {
            new FoodConsumptionTemplate()
        };
        simulation.AddStage(consumptionStage);
        
        simulation.Initialize();

        population.AddToCurrentInventory(EcItem.FOOD, 3);
        population.AddToCurrentInventory(EcItem.POPULATION, 1);
        population.AddToCurrentInventory(EcItem.LAND_OWNERSHIP, 1);
        Debug.Log("Simulation initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = simulation.DebugString();
    }

    public void Step()
    {
        Debug.Log("Simulation stepping...");
        simulation.StepInventory();
        Debug.Log("Simulation stepped.");
        population.AddToCurrentInventory(EcItem.FOOD, Random.Range(1, 3));
    }

    public void Optimize(int n)
    {
        Debug.Log("Simulation optimizing...");
        for (int i = 0; i < n; i++)
        {
            simulation.Optimize();
        }
        Debug.Log("Simulation optimized.");
    }
}
