using UnityEngine;
using System.Collections;

public class PowerLogic : MonoBehaviour
{
    public Cable cable;
    public PlayerInteractions playerInteractions;
    public Vector3 plugPosition = new Vector3(12.131f, 1.211f, -18.35f);
    public Quaternion plugRotation = Quaternion.Euler(0f, 0f, 0f);

    public Light directionalLight; // <-- Add this reference
    [SerializeField] private AudioClip powerOnMp;
    [SerializeField] private AudioSource audioSource;
    public Objectives objectives;

    public bool powerOn = false;

    public int oxygen = 10;
    private Coroutine oxygenDepletionRoutine; // To track the coroutine

    public VRTextDisplay vrTextDisplay; // Assign this in the Inspector
    public Desk desk;  // Reference to Desk to check wind turbines
    public bool win = false;

    private void Update()
    {
        if (cable == null || playerInteractions == null)
            return;
    }

    public void HandleGrabAction()
    {
        Debug.Log("PowerLogic: HandleGrabAction()");

        if (cable.isInElectricBoxTrigger)
        {
            TryActivatePower();
        }
        else
        {
            Debug.Log("PowerLogic: Conditions not met to activate power.");
        }
    }

    private void TryActivatePower()
    {
        if (cable.isPlugged)
        {
            Debug.Log("PowerLogic: Cable already plugged.");
            return;
        }

        TurnOnPower();
    }

    private void TurnOnPower()
    {
        powerOn = true;
        cable.PlugInto(plugPosition, plugRotation);
        objectives.SetObjective_MineFossils();
        Debug.Log("PowerLogic: Power ON!");

        if (audioSource != null && powerOnMp != null)
            audioSource.PlayOneShot(powerOnMp);

        if (directionalLight != null)
            directionalLight.intensity = 1f;

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
            Destroy(door);

        Debug.Log("AudioSource is " + (audioSource == null ? "null" : "set"));

        // Start oxygen depletion
        if (oxygenDepletionRoutine == null)
            oxygenDepletionRoutine = StartCoroutine(OxygenDrainRoutine());
    }

    private IEnumerator OxygenDrainRoutine()
    {
        while (powerOn && oxygen > 0)
        {
            yield return new WaitForSeconds(4f);
            oxygen -= 1;

            // Clamp oxygen to max 100
            if (oxygen > 100)
            {
                oxygen = 100;
            }

            // Win condition: all turbines on AND oxygen >= 100
            if (desk != null && desk.turbineActive.Length == 3 &&
                desk.turbineActive[0] && desk.turbineActive[1] && desk.turbineActive[2] &&
                oxygen >= 100)
            {
                win = true;
                if (vrTextDisplay != null)
                {
                    vrTextDisplay.ShowWinMessage(); // Show win message
                }

                // Wait 3 seconds, then restart the scene
                yield return new WaitForSeconds(3f);
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                yield break;
            }
        }

        // Loss condition
        if (oxygen <= 0)
        {
            if (vrTextDisplay != null)
            {
                vrTextDisplay.ShowFailureMessage();
            }

            yield return new WaitForSeconds(3f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}