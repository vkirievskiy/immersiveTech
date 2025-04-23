using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;  // Assign this in the Inspector
    [SerializeField] private float spawnDelay = 1f;    // Delay before deciding to spawn stone
    [SerializeField] private float cooldownDuration = 3f; // Time before it can be hit again
    [SerializeField] private float spawnChance = 0.5f; // 0.5 = 50% chance
    [SerializeField] private AudioClip rockBreak;
    [SerializeField] private AudioSource audioSource;

    private bool isOnCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOnCooldown) return;

        if (other.gameObject.CompareTag("Pickaxe"))
        {
            StartCoroutine(HandlePickaxeHit());
        }
    }

    private IEnumerator HandlePickaxeHit()
    {
        isOnCooldown = true;

        // Wait before deciding if a stone spawns
        yield return new WaitForSeconds(spawnDelay);

        float roll = Random.value;
        if (roll <= spawnChance)
        {
            Instantiate(stonePrefab, transform.position, Quaternion.identity);
            FindObjectOfType<Objectives>().CompleteCurrentObjective();
            if (audioSource != null && rockBreak != null)
                audioSource.PlayOneShot(rockBreak);
            Debug.Log("Stone spawned!");
        }
        else
        {
            Debug.Log("Nothing spawned this time.");
        }

        // Wait before re-enabling detection
        yield return new WaitForSeconds(cooldownDuration - spawnDelay);
        isOnCooldown = false;
    }
}