using UnityEngine;

public class TestSimulation : MonoBehaviour
{
    EcSimulation simulation;
    public TMPro.TMP_Text debugText;

    void Start()
    {
        Debug.Log("Simulation initializing...");
        simulation = new EcSimulation();
        simulation.AddPopulation(new EcPopulation());
        simulation.AddStage(new EcStage());
        simulation.Initialize();
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
        simulation.Step();
        Debug.Log("Simulation stepped.");
    }
}
