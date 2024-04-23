using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Ammo : MonoBehaviour
{
    [SerializeField] protected float destroyDelay = 10f;

    public float moveSpeed = 10f;
    [HideInInspector] public Vector3 moveDir = Vector3.forward;
    [SerializeField] protected float knockbackForce = 15f;
    protected Rigidbody rb;

    protected virtual void Start()
    {
        DestroyAfterDelay();
        rb = GetComponent<Rigidbody>();
        rb.velocity = moveDir * moveSpeed;
    }

    protected virtual void DestroyAfterDelay()
    {
        Destroy(gameObject, destroyDelay);
    }
}
