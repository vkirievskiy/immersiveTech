using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectives : MonoBehaviour
{
    public TextMeshProUGUI objectiveTextUI;

    public List<string> objectiveList = new List<string>()
    {
        "Switch on the power",
        "Mine fossil fuels from the asteroid",
        "Melt fossil fuels to create toxic waste barrels",
        "Place the toxic waste barrels on the blue desk"
    };

    private int currentObjectiveIndex = 0;

    void Start()
    {
        ShowCurrentObjective();
    }

    void ShowCurrentObjective()
    {
        if (objectiveTextUI != null && currentObjectiveIndex < objectiveList.Count)
        {
            objectiveTextUI.text = "Objective: " + objectiveList[currentObjectiveIndex];
            Debug.Log("Current Objective: " + objectiveList[currentObjectiveIndex]);
        }
        else
        {
            objectiveTextUI.text = "All objectives completed! Powering turbines...";
            Debug.Log("All objectives completed!");
        }
    }

    public void CompleteCurrentObjective()
    {
        Debug.Log("Completed: " + objectiveList[currentObjectiveIndex]);

        currentObjectiveIndex++;
        ShowCurrentObjective();
    }

    public string GetCurrentObjective()
    {
        return currentObjectiveIndex < objectiveList.Count ? objectiveList[currentObjectiveIndex] : "All objectives completed!";
    }

    // ----- Individual Functions -----
    public void SetObjective_PowerOn()
    {
        currentObjectiveIndex = 0;
        ShowCurrentObjective();
    }

    public void SetObjective_MineFossils()
    {
        currentObjectiveIndex = 1;
        ShowCurrentObjective();
    }

    public void SetObjective_MeltFossils()
    {
        currentObjectiveIndex = 2;
        ShowCurrentObjective();
    }

    public void SetObjective_PlaceBarrels()
    {
        currentObjectiveIndex = 3;
        ShowCurrentObjective();
    }
}