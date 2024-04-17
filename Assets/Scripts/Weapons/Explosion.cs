using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 3000f;
    public float explosionUpForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            ApplyExplosionForce();
        }
    }

    void ApplyExplosionForce()
    {
        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // Apply explosive force to each nearby rigidbody
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = collider.transform.position - transform.position;
                rb.AddForce(transform.TransformDirection(Vector3.up) * explosionUpForce, ForceMode.Impulse);

                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}