using UnityEngine;

public class Furnace : MonoBehaviour
{
    [SerializeField] private GameObject barrelPrefab;
    public int rockCounter = 0;
    public Objectives objectives;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            rockCounter++;
            Debug.Log("Rock added to desk. Total: " + rockCounter);

            Destroy(collision.gameObject); // Destroy the rock that touched the desk

            if (rockCounter >= 3)
            {
                Vector3 spawnPosition = new Vector3(9f, 0.27f, -1.11f);
                Instantiate(barrelPrefab, spawnPosition, Quaternion.identity);
                rockCounter = 0; // Reset counter after spawning
                objectives.SetObjective_PlaceBarrels();
            }
        }
    }

    public int GetRockCount()
{
    return rockCounter;
}
}