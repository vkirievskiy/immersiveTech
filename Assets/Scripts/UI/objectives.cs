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
        string current = objectiveList[currentObjectiveIndex];
        Debug.Log("Current Objective: " + current);

        if (objectiveTextUI != null)
        {
            objectiveTextUI.text = "Objective: " + current;
        }
    }

    public void CompleteCurrentObjective()
    {
        Debug.Log("Completed: " + objectiveList[currentObjectiveIndex]);

        currentObjectiveIndex++;

        if (currentObjectiveIndex < objectiveList.Count)
        {
            ShowCurrentObjective();
        }
        else
        {
            if (objectiveTextUI != null)
            {
                objectiveTextUI.text = "All objectives completed! Powering turbines...";
            }
            Debug.Log("All objectives completed!");
        }
    }

    public string GetCurrentObjective()
    {
        return objectiveList[currentObjectiveIndex];
    }
}