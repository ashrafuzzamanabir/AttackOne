using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab; // Rock prefab to spawn
    [SerializeField] private List<Transform> spawnPoints; // List of predefined spawn points
    [SerializeField] private float spawnIntervalMin = 3f; // Minimum spawn interval
    [SerializeField] private float spawnIntervalMax = 5f; // Maximum spawn interval

    private bool isSpawning = true;


    private void Start()
    {
        if (rockPrefab == null)
        {
            Debug.LogError("Rock Prefab is missing! Assign it in the Inspector.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned! Add spawn points in the Inspector.");
            return;
        }

        // Start spawning rocks
        StartCoroutine(SpawnRocks());
    }

    private IEnumerator SpawnRocks()
    {
        while (isSpawning)
        {
            float interval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(interval);

            SpawnRock();
        }
    }

    private void SpawnRock()
    {
        // Randomly pick one of the predefined spawn points
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject spawnedRock = Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);

        // Ensure the rock has a Rigidbody2D for falling
        if (!spawnedRock.GetComponent<Rigidbody2D>())
        {
            spawnedRock.AddComponent<Rigidbody2D>(); // Add Rigidbody2D if missing
        }

        Debug.Log($"Rock spawned at {spawnPoint.position}");
    }

    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("Rock spawning stopped.");
    }

}
