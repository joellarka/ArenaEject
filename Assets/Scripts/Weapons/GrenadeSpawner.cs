using System.Collections;
using UnityEngine;

public class GrenadeSpawner : MonoBehaviour
{
    public GameObject grenadePrefab;
    public float spawnInterval = 30f;
    public float spawnRadius = 10f;
    public float spawnHeight = 5f;

    void Start()
    {
        InvokeRepeating("SpawnGrenade", 0f, spawnInterval);
    }

    void SpawnGrenade()
    {
        float theta = Random.Range(0f, 2f * Mathf.PI);
        float phi = Random.Range(0f, Mathf.PI);

        float x = spawnRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = spawnHeight;
        float z = spawnRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

        Vector3 randomPosition = new Vector3(x, y, z) + transform.position;
        GameObject grenade = Instantiate(grenadePrefab, randomPosition, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}