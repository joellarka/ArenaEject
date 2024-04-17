using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KnockBackHandler : MonoBehaviour
{
    private Rigidbody rb;

    public float frictionCoefficient = 5;
    public float frictionDelay = 5f;
    private bool knockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!knockedBack) return; 

        if(rb != null)
        {
            if (rb.velocity.sqrMagnitude > 0)
            {
                Deaccelerate();
            }
        }
    }

    public void Deaccelerate()
    {
        rb.velocity -= frictionCoefficient * Time.deltaTime * rb.velocity.normalized;
        if (rb.velocity.sqrMagnitude < 1f) // Check if velocity is close to zero
        {
            rb.velocity = Vector3.zero; // Ensure velocity is exactly zero
            if (TryGetComponent<Movement>(out Movement myMovement))
            {
                myMovement.enabled = true;
                knockedBack = false;
            }
        }
    }

    public void GetKnockedBack(Vector3 dir, float force)
    {
        if(TryGetComponent<Movement>(out Movement myMovement))
        {
            myMovement.enabled = false;
        }

        knockedBack = true;
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
