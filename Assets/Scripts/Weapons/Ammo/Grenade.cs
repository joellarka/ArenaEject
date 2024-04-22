using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Ammo
{
    [SerializeField] protected bool explodeOnImpact = false;
    [SerializeField] protected float explodeTime = 1.5f;
    [SerializeField] protected GameObject explosionPrefab;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = moveDir * moveSpeed;

        this.Invoke(nameof(Explode), explodeTime);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<KnockBackHandler>(out KnockBackHandler hit))
        {   
            // apply knockback if moving fast enough
            if (rb.velocity.sqrMagnitude > 2f)
            {
                Vector3 dir = rb.velocity.normalized;
                dir.y = 0;
                hit.GetKnockedBack(dir, knockbackForce);
            }
        }

        if (!explodeOnImpact) return;

        Explode();
    }

    private void Explode()
    {
        this.CancelInvoke();

        if(explosionPrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        GameObject explosionObj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
