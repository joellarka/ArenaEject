using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PickUp pickUpPrefab;
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private float spawnInterval = 10f;
    private PickUp myCurrentPickUp;

    private void Start()
    {
        InvokeRepeating(nameof(AttemptSpawn), spawnDelay, spawnInterval);
    }


    private void AttemptSpawn()
    {
        if (myCurrentPickUp != null) return;

        myCurrentPickUp = Instantiate(pickUpPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
