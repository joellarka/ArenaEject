using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyDelay = 3f;
    public float knockbackForce = 15f;
    public float knockbackDuration = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        if (!GetComponent<Collider>().isTrigger)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.transform.name);
        if (collision.gameObject.TryGetComponent<KnockBackHandler>(out KnockBackHandler hit))
        {
            Vector3 dir = rb.velocity.normalized;
            dir.y = 0;
            hit.GetKnockedBack(dir, knockbackForce);
        }

        Destroy(gameObject);
    }
}