using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ClusterSpawner : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public int numberOfClusters = 5;
    public int minElementsInCluster = 3;
    public int maxElementsInCluster = 10;
    public float clusterRadius = 20;

    // Use this function to start the spawning process.
    public void SpawnClusters()
    {
        if (prefabs.Count == 0)
        {
            Debug.LogWarning("No prefabs added to spawn!");
            return;
        }

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        for (int i = 0; i < numberOfClusters; i++)
        {
            // Get a central point for the cluster.
            Vector2 centralPoint = new Vector2(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y)
            );

            // Determine how many elements in this cluster.
            int elementsInThisCluster = Random.Range(minElementsInCluster, maxElementsInCluster + 1);
            
            for (int j = 0; j < elementsInThisCluster; j++)
            {
                // Spawn a random prefab from the list around the central point.
                GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Count)];
                Vector2 spawnPosition = centralPoint + Random.insideUnitCircle * clusterRadius;
                Instantiate(randomPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}