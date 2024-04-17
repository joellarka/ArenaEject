using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFriction : MonoBehaviour
{
    private Rigidbody rb;

    public float frictionCoefficient = 5;
    public float frictionDelay = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(rb != null)
        {
            if (rb.velocity.magnitude > 0)
            {
                DeacceleratePlayer();
            }
        }
    }

    public void DeacceleratePlayer()
    {
        rb.velocity -= frictionCoefficient * Time.deltaTime * rb.velocity.normalized;
        if (rb.velocity.sqrMagnitude < 0.01f) // Check if velocity is close to zero
        {
            rb.velocity = Vector3.zero; // Ensure velocity is exactly zero
        }
    }
}
