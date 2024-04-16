using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFriction : MonoBehaviour
{
    private Rigidbody rb;

    public float frictionCoefficient = 50;

    public void DeacceleratePlayer()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity -= frictionCoefficient * Time.deltaTime * rb.velocity.normalized;
        if (rb.velocity.sqrMagnitude < 0.01f) // Check if velocity is close to zero
        {
            rb.velocity = Vector3.zero; // Ensure velocity is exactly zero
        }
    }
}
