using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 3000f;
    public float explosionUpForce = 10f;
    public float duration = 0.1f;
    public GameObject explosionPrefab;

    private bool canExplode = true;
    private float explosionStartTime;
    private GameObject explosionObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Explode();
        }
    }

    void Explode()
    {
        if(canExplode)
        {
            explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            explosionStartTime = Time.time;

            canExplode = false;

            ApplyExplosionForce();
            StartCoroutine(DestroyAfterDuration(duration));
        }
    }

    void ApplyExplosionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

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

    void Update()
    {
        if (explosionObject != null && Time.time < explosionStartTime + duration)
        {
            float progress = (Time.time - explosionStartTime) / duration;

            float scale = Mathf.Lerp(0f, explosionRadius, progress);
            explosionObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    IEnumerator DestroyAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        canExplode = true;
        Destroy(explosionObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}