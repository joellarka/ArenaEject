using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Vector3 respawnPoint;

    private void Start()
    {
        respawnPoint = new Vector3(0, 3, 15);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Movement player = other.GetComponent<Movement>();
        
        if (player != null)
        {
            playerObject.transform.position = respawnPoint;
            Console.WriteLine("Player has respawned!");
        }
    }
}
