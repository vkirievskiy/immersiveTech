using UnityEngine;
using System.Collections;

public class Desk : MonoBehaviour
{
    [SerializeField] public GameObject[] windTurbines;  // Assign your 3 wind turbines in the Inspector
    public bool[] turbineActive; // Track which turbines are active
    public int barrelCount = 0;
    [SerializeField] private float spinSpeed = 100f; // Degrees per second

    // Reference to PowerLogic to modify oxygen level
    public PowerLogic powerLogic; // Assign this in the Inspector

    private Coroutine oxygenIncreaseRoutine;  // To track the oxygen increase routine

    public void Start()
    {
        turbineActive = new bool[windTurbines.Length];
    }

    private void Update()
    {
        RotateActiveTurbines();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Barrel"))
        {
            barrelCount++;
            Debug.Log("Barrel processed by furnace. Total barrels: " + barrelCount);
            FindObjectOfType<Objectives>().CompleteCurrentObjective();

            Destroy(collision.gameObject);

            UpdateWindTurbines();
        }
    }

    private void UpdateWindTurbines()
    {
        for (int i = 0; i < windTurbines.Length; i++)
        {
            if (i < barrelCount && !turbineActive[i])
            {
                turbineActive[i] = true;
                Debug.Log($"WindTurbine {i + 1} activated.");
            }
        }

        // Start or restart the oxygen increase routine if turbines are active
        if (oxygenIncreaseRoutine == null && turbineActive.Length > 0)
        {
            oxygenIncreaseRoutine = StartCoroutine(IncreaseOxygenRoutine());
        }
    }

    private void RotateActiveTurbines()
    {
        for (int i = 0; i < windTurbines.Length; i++)
        {
            if (turbineActive[i] && windTurbines[i] != null)
            {
                // Rotate around Z-axis only
                windTurbines[i].transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator IncreaseOxygenRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);  // Wait for 5 seconds before increasing oxygen

            int activeTurbines = 0;

            // Count how many turbines are active
            for (int i = 0; i < turbineActive.Length; i++)
            {
                if (turbineActive[i])
                {
                    activeTurbines++;
                }
            }

            // Increase oxygen based on the number of active turbines
            int oxygenIncrease = 0;
            if (activeTurbines == 1)
            {
                oxygenIncrease = 1;  // 1 turbine = +1 oxygen
            }
            else if (activeTurbines == 2)
            {
                oxygenIncrease = 3;  // 2 turbines = +3 oxygen
            }
            else if (activeTurbines >= 3)
            {
                oxygenIncrease = 5;  // 3 or more turbines = +5 oxygen
            }

            // Apply the oxygen increase
            if (powerLogic != null)
            {
                powerLogic.oxygen += oxygenIncrease;
                Debug.Log($"Oxygen increased by {oxygenIncrease}. Current oxygen: {powerLogic.oxygen}");
            }

            // Stop the routine if all turbines are inactive
            bool anyTurbineActive = false;
            foreach (bool isActive in turbineActive)
            {
                if (isActive)
                {
                    anyTurbineActive = true;
                    break;
                }
            }
            if (!anyTurbineActive) break;  // Exit the loop if no turbines are active
        }
    }
}