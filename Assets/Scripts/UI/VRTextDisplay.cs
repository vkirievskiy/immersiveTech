using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VRTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    [Header("References")]
    public PowerLogic powerLogic;
    public Furnace furnace;
    public Desk desk;
    private bool gameEnded = false;
    public int turbineCount = 0;

    private void Update()
    {
        if (!gameEnded)
        {
            ShowStatus();
        }
    }

    public void ShowStatus()
    {
        if (powerLogic == null || furnace == null || desk == null || textUI == null)
            return;

        if (powerLogic.oxygen <= 0)
        {
            textUI.text = "You failed!";
            Debug.Log("You fail");
            return;
        }

        if (powerLogic.win)
        {
            textUI.text = "You Won!";
            Debug.Log("You won");
            return;
        }

        turbineCount = 0; // <- Reset count before counting again
        for (int i = 0; i < desk.turbineActive.Length; i++)
        {
            if (desk.turbineActive[i])
                turbineCount++;
        }

        string statusText =
            $"1. Power is {(powerLogic.powerOn ? "On" : "Off")}\n" +
            $"2. Fossil Fuels: {furnace.GetRockCount()}/3\n" +
            $"3. Oxygen level: {powerLogic.oxygen}/100\n" +
            $"4. Turbines active: {turbineCount}/3";

        textUI.text = statusText;
    }
    public void ShowWinMessage()
    {
        gameEnded = true;
        StopAllCoroutines();

        int barrelsProduced = desk != null ? desk.barrelCount : 0;

        textUI.text = $"You won!\nYou managed to produce {barrelsProduced} toxic waste barrels.";
        textUI.gameObject.SetActive(true);
    }

    public void ShowFailureMessage()
    {
        gameEnded = true;
        StopAllCoroutines();
        textUI.text = "You failed!";
        textUI.gameObject.SetActive(true);
    }
}